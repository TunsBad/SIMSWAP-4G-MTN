using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using sim_swap.Extensions;
using sim_swap.Models;
using sim_swap.Security;
using System;
using System.Web;
using Microsoft.Extensions.Configuration;

namespace sim_swap.Controllers
{
    [Produces("application/json")]

    public class UserController : Controller
    {
        private Authorization _authorization;
        private readonly ConnectionManager _connectionManager;
        private readonly Logger _logger;
        private readonly IConfiguration _configuration;

        public UserController(ConnectionManager connectionManager, Logger logger, Authorization authorization, IConfiguration configuration)
        {
            _connectionManager = connectionManager;
            _logger = logger;
            _authorization = authorization;
            _configuration = configuration;
        }

        [HttpGet]
        public string Welcome()
        {
            return "Welcome To Sim Swap API";
        }

        [HttpGet]
        [MiddlewareFilter(typeof(TokenHeaderAuthorizationPipeline))]
        public string Logout(string authToken)
        {
            if (authToken == null)
            {
                authToken = Request.Headers["Authorization"];
            }

            var result = _connectionManager.CreateCommand("logout")
                                   .WithSqlParam("token", authToken)
                                   .ExecuteReturningScalarAsync<int>();
            if (result.Result > 0)
            {
                return "You logged out";
            }
            else
            {
                return "Couldn't log user out, Please try again";
            }
        }

        [HttpPost]
        public AjaxResponse LogIn([FromBody]LoginDetail user)
        {
            var webReq = Request;
            //if (_authorization.AuthenticateRequest(user.TimeStamp, webReq))
            //{
            Console.Write("Success!");

            try
            {
                var result = _connectionManager.CreateCommand("login")
                               .WithSqlParam("username", user.Username)
                               .WithSqlParam("password", user.Password)
                               .ExecuteStoredProc<LoginDetail>();

                if (result.Count > 0)
                {
                    var auth_token = Guid.NewGuid().ToString();
                    DateTime expiryDate = new DateTime(DateTime.Now.AddHours(1).Ticks);

                    _connectionManager.CreateCommand("saveusertoken")
                        .WithSqlParam("username", user.Username)
                        .WithSqlParam("user_token", auth_token)
                        .WithSqlParam("expirydate", expiryDate, NpgsqlTypes.NpgsqlDbType.Timestamp)
                        .ExecuteNonReturning();

                    return (new AjaxResponse
                    {
                        Success = true,
                        Exception = null,
                        Message = "Successfully Logged user In",
                        LoginData = new LoginResponse
                        {
                            UserId = result[0].UserId,
                            Username = user.Username,
                            AuthToken = auth_token,
                            TokenExpiryDate = expiryDate
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                _logger.logError(ex);
                return (new AjaxResponse
                {
                    Success = false,
                    Exception = ex.ToString(),
                    Message = "Login Failed",
                    LoginData = null
                });
            }
            //}

            _logger.logError("LogIn Failed");
            return (new AjaxResponse
            {
                Success = false,
                Exception = null,
                Message = "Login Failed",
                LoginData = null
            });
        }

        [HttpPost]
        public AjaxResponse PinCodeLogIn([FromBody]LoginDetail user)
        {
            try
            {
                var result = _connectionManager.CreateCommand("pincodelogin")
                               .WithSqlParam("phonenumber", user.Msisdn)
                               .WithSqlParam("pincode", user.UserPin)
                               .ExecuteStoredProc<PhoneLogin>();

                if (result.Count > 0)
                {
                    _connectionManager.CreateCommand("expirepincode")
                        .WithSqlParam("phonenumber", user.Msisdn)
                        .WithSqlParam("pincode", user.UserPin)
                        .ExecuteNonReturning();

                    var auth_token = Guid.NewGuid().ToString();
                    DateTime expiryDate = new DateTime(DateTime.Now.AddHours(1).Ticks);

                    string username = _connectionManager.CreateCommand("savetoken_for_pincode")
                        .WithSqlParam("phonenumber", user.Msisdn)
                        .WithSqlParam("token", auth_token)
                        .WithSqlParam("expirydate", expiryDate, NpgsqlTypes.NpgsqlDbType.Timestamp)
                        .ExecuteScalar().ToString();

                    return (new AjaxResponse
                    {
                        Success = true,
                        Exception = null,
                        Message = "Successfully Logged user In",
                        LoginData = new LoginResponse
                        {
                            Username = username,
                            AuthToken = auth_token,
                            TokenExpiryDate = expiryDate
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                _logger.logError(ex);
                return (new AjaxResponse
                {
                    Success = false,
                    Exception = ex.ToString(),
                    Message = "Login Failed, use another Pin Code",
                    LoginData = null
                });
            }
            //}

            _logger.logError("LogIn Failed");
            return (new AjaxResponse
            {
                Success = false,
                Exception = null,
                Message = "Login Failed",
                LoginData = null
            });
        }


        [HttpGet]
        [HttpPost]
        public AjaxResponse RequestLoginTokenForPhone(string msisdn)
        {
            string shortCode = _configuration.GetValue<string>("ShortCode");
            DateTime creationDate = DateTime.Now;
            DateTime expiryDate = new DateTime(DateTime.Now.AddHours(1).Ticks);
            int pinCode = new Random().Next(100000, 999999);

            try
            {
                var response = _connectionManager.CreateCommand("savephonepins")
                             .WithSqlParam("msisdn", msisdn, NpgsqlTypes.NpgsqlDbType.Text)
                             .WithSqlParam("pin", pinCode.ToString(), NpgsqlTypes.NpgsqlDbType.Text)
                             .WithSqlParam("datecreated", creationDate, NpgsqlTypes.NpgsqlDbType.Timestamp)
                             .WithSqlParam("expirydate", expiryDate, NpgsqlTypes.NpgsqlDbType.Timestamp)
                             .ExecuteStoredProc<int>();

                if (response != null)
                {
                    string message = "Your new Pin is " + pinCode + ", It will expires in an hour @ " + expiryDate;

                    _connectionManager.CreateSmsCommand("schedule_sms_message")
                                 .WithSqlParam("phonenumber", msisdn, NpgsqlTypes.NpgsqlDbType.Text)
                                 .WithSqlParam("shortcode", shortCode, NpgsqlTypes.NpgsqlDbType.Text)
                                 .WithSqlParam("message", message, NpgsqlTypes.NpgsqlDbType.Text)
                                 .ExecuteNonReturning();

                    return new AjaxResponse
                    {
                        Success = true,
                        Message = "PIN code will be received by SMS shortly"
                    };
                }

                return new AjaxResponse
                {
                    Success = false,
                    Message = "An error Occured, Pin code not sent !"
                };
            }
            catch (Exception ex)
            {
                return new AjaxResponse
                {
                    Success = false,
                    Message = ex.ToString()
                };
            }

        }
    }
}

