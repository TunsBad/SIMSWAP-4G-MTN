using Microsoft.AspNetCore.Mvc;
using sim_swap.Extensions;
using sim_swap.Models;
using System;
using System.Net.Http;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net;

namespace sim_swap.Controllers
{
    [Produces("application/json")]
    [MiddlewareFilter(typeof(TokenHeaderAuthorizationPipeline))]
    public class VerifyUserController : Controller
    {
        private readonly ConnectionManager _connectionManager;
        private readonly Logger _logger;

        public VerifyUserController(ConnectionManager connectionManager, Logger logger)
        {
            _connectionManager = connectionManager;
            _logger = logger;
        }

        [HttpGet]
        [HttpPost]
        public async Task<AjaxResponse> Verify(string phonenumber)
        {
            try
            {
                VerifyResponse SubscriberDetail = await GetSubscriberDetail(phonenumber);

                if (SubscriberDetail == null || string.IsNullOrEmpty(SubscriberDetail.FullName))
                {
                    return (new AjaxResponse
                    {
                        Success = false,
                        Message = "Subscriber Not Found",
                        LoginData = null,
                        SubscriberDetails = null
                    });
                }

                return (new AjaxResponse
                {
                    Success = true,
                    Exception = null,
                    Message = "Verification Successful",
                    LoginData = null,
                    SubscriberDetails = SubscriberDetail
                });
            }
            catch (Exception ex)
            {
                _logger.logError(ex);
                return (new AjaxResponse
                {
                    Success = false,
                    Exception = ex.ToString(),
                    Message = "Verification Failed",
                    LoginData = null,
                    SubscriberDetails = null
                });
            };
        }

        private async Task<VerifyResponse> GetSubscriberDetail(string phonenumber)
        {
            var formattedPhoneNumber = phonenumber.Length == 9 ? "233" + phonenumber
                : (phonenumber.Length == 10 ? "233" + phonenumber.Substring(1) : phonenumber);

            string baseUrl = "http://10.135.14.96:8011/Request/GetSimDetail?msisdn=" + formattedPhoneNumber;
            using (WebClient client = new WebClient())
            {
                //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                //Content - Type 

                client.Headers.Add("Content-Type:application/json");
                client.Headers.Add("Accept:application/json");

                string response = await client.DownloadStringTaskAsync(baseUrl);

                if (response != null)
                {
                    var responseAsConcreteType = JsonConvert.DeserializeObject<VerifyResponse>(response);

                    return responseAsConcreteType;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}