using Microsoft.AspNetCore.Mvc;
using sim_swap.Extensions;
using sim_swap.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Drawing.Imaging;
using Microsoft.Extensions.Configuration;
using System.Web;
using Microsoft.AspNetCore.Hosting;
using System.Text.RegularExpressions;

namespace sim_swap.Controllers
{
    [Produces("application/json")]

    public class SimSwapController : Controller
    {
        private readonly ConnectionManager _connectionManager;
        private readonly Logger _logger;
        private string BASE_API_URL = "";
        private readonly IHostingEnvironment _hostingEnvironment;


        public SimSwapController(ConnectionManager connectionManager, Logger logger, IHostingEnvironment hostingEnvironment)
        {
            _connectionManager = connectionManager;
            _logger = logger;
            BASE_API_URL = _logger._configuration.GetValue<string>("BASE_API_URL");
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        [MiddlewareFilter(typeof(TokenHeaderAuthorizationPipeline))]
        public AjaxResponse GetSimSwapRequests(int userid)
        {
            IList<RequestResponse> request;
            try
            {
                request = _connectionManager.CreateCommand("getswaprequests")
                              .WithSqlParam("userid", userid, NpgsqlTypes.NpgsqlDbType.Bigint)
                              .ExecuteStoredProc<RequestResponse>();

                foreach (var item in request)
                {
                    var IDCardImage = new FileDetail()
                    {
                        FileName = "IdCardImageFor-" + item.Id + "-" + item.Msisdn,
                        FileUrl = BASE_API_URL + "getidcardimage?requestid=" + item.Id + "&authToken=" + GetCurrentToken()
                    };
                    var RequesterImage = new FileDetail()
                    {
                        FileName = "RequesterImageFor-" + item.Id + "-" + item.Msisdn,
                        FileUrl = BASE_API_URL + "getrequesterimage?requestid=" + item.Id  + "&authToken=" + GetCurrentToken()
                    };
                    item.Attachment = new Attachment()
                    {
                        IdCardImage = IDCardImage,
                        RequesterImage = RequesterImage
                    };
                }

            }
            catch (Exception ex)
            {
                _logger.logError(ex);
                return new AjaxResponse
                {
                    Success = false,
                    Exception = ex.ToString(),
                    Message = "Failed to get list of sim swap Requests",
                    LoginData = null,
                    SubscriberDetails = null,
                    SwapRequests = null
                };
            }
            return (new AjaxResponse
            {
                Success = true,
                Exception = null,
                Message = "List of all sim swap Requests",
                LoginData = null,
                SubscriberDetails = null,
                SwapRequests = request
            });

        }

        private Microsoft.Extensions.Primitives.StringValues GetCurrentToken()
        {
            string authHeader = Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(authHeader))
            {
                return Request.Query["authToken"];
            }
            return authHeader;
        }

        [HttpGet]
        [MiddlewareFilter(typeof(TokenHeaderAuthorizationPipeline))]
        public AjaxResponse GetSimSwapRequestByStatus(int userid, string status)
        {
            IList<RequestResponse> request;
            try
            {
                request = _connectionManager.CreateCommand("getswaprequestsbystatus")
                              .WithSqlParam("userid", userid, NpgsqlTypes.NpgsqlDbType.Bigint)
                              .WithSqlParam("status", status, NpgsqlTypes.NpgsqlDbType.Text)
                              .ExecuteStoredProc<RequestResponse>();

                foreach (var item in request)
                {
                    var IDCardImage = new FileDetail()
                    {
                        FileName = "IdCardImageFor-" + item.Id + "-" + item.Msisdn,
                        FileUrl = BASE_API_URL + "getidcardimage?requestid=" + item.Id + "&authToken=" + GetCurrentToken()
                    };
                    var RequesterImage = new FileDetail()
                    {
                        FileName = "RequesterImageFor-" + item.Id + "-" + item.Msisdn,
                        FileUrl = BASE_API_URL + "getrequesterimage?requestid=" + item.Id + "&authToken=" + GetCurrentToken()
                    };
                    item.Attachment = new Attachment()
                    {
                        IdCardImage = IDCardImage,
                        RequesterImage = RequesterImage
                    };
                }

            }
            catch (Exception ex)
            {
                _logger.logError(ex);
                return new AjaxResponse
                {
                    Success = false,
                    Exception = ex.ToString(),
                    Message = "Failed to get list of sim swap Requests",
                    LoginData = null,
                    SubscriberDetails = null,
                    SwapRequests = null
                };
            }
            return (new AjaxResponse
            {
                Success = true,
                Exception = null,
                Message = "List of all sim swap Requests",
                LoginData = null,
                SubscriberDetails = null,
                SwapRequests = request
            });

        }

        //public AttachmentResponse GetRequesterImages(string msisdn)
        //{
        //    try
        //    {

        //        var response = _connectionManager.CreateCommand("getattachments")
        //            .WithSqlParam("customermsisdn", msisdn)
        //            .ExecuteStoredProc<AttachmentResponse>().SingleOrDefault();

        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.Write(ex);
        //        _logger.logError(ex);

        //        return null;
        //    }
        //}

        [HttpGet]
        [MiddlewareFilter(typeof(TokenHeaderAuthorizationPipeline))]
        public ActionResult GetIdCardImage(int requestid)
        {
            try
            {
                byte[] idcardimageArray;

                var response = _connectionManager.CreateCommand("getattachments")
                    .WithSqlParam("requestid", requestid, NpgsqlTypes.NpgsqlDbType.Bigint)
                    .ExecuteStoredProc<AttachmentResponse>().SingleOrDefault();

                response.IdCardImage.Replace(Environment.NewLine, "").Replace("\n", "");

                if (response.IdCardImage.StartsWith("data:image/jpeg;base64,"))
                {
                    idcardimageArray = Convert.FromBase64String(response.IdCardImage.Replace("data:image/jpeg;base64,", string.Empty));
                }
                else
                {
                    idcardimageArray = Convert.FromBase64String(response.IdCardImage);
                }

                using (MemoryStream ms = new MemoryStream(idcardimageArray))
                {
                    Image.FromStream(ms);

                    return File(ms.ToArray(), "image/jpeg");
                }

            }
            catch (Exception ex)
            {
                Console.Write(ex);
                _logger.logError(ex);

                return StatusCode(500);
            }

        }

        [HttpGet]
        [MiddlewareFilter(typeof(TokenHeaderAuthorizationPipeline))]
        public ActionResult GetRequesterImage(int requestid)
        {
            try
            {
                byte[] requesterimageArray;

                var response = _connectionManager.CreateCommand("getattachments")
                    .WithSqlParam("requestid", requestid, NpgsqlTypes.NpgsqlDbType.Bigint)
                    .ExecuteStoredProc<AttachmentResponse>().SingleOrDefault();

                response.RequesterImage.Replace(Environment.NewLine, "").Replace("\n", "");

                if (response.RequesterImage.StartsWith("data:image/jpeg;base64,"))
                {
                    requesterimageArray = Convert.FromBase64String(response.RequesterImage.Replace("data:image/jpeg;base64,", string.Empty));
                }
                else
                {
                    requesterimageArray = Convert.FromBase64String(response.RequesterImage);
                }

                using (MemoryStream ms = new MemoryStream(requesterimageArray))
                {
                    Image.FromStream(ms);

                    return File(ms.ToArray(), "image/jpeg");
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                _logger.logError(ex);

                return StatusCode(500);
            }
        }

        [HttpPost]
        [MiddlewareFilter(typeof(TokenHeaderAuthorizationPipeline))]
        public SubmitResponse SubmitSimSwapRequest([FromBody] RequestModel request)
        {
            int success = 0;

            var ValidateResponse = ValidateIdNumber(request.IdType, request.IdNumber);

            if(ValidateResponse.Success == false)
            {
                return (new SubmitResponse
                {
                    Success = false,
                    Message = ValidateResponse.Message,
                });
            }

            if (!CheckAllDigits(request.NewSimSerial))
            {
                SubmitResponse response = new SubmitResponse()
                {
                    Success = false,
                    Message = "Invalid Serial Number"
                };

                return response;
            }

            if (!CheckCoordinates(request.Latitude, request.Longitude))
            {
                SubmitResponse response = new SubmitResponse()
                {
                    Success = false,
                    Message = "Invalid Longitude or Latitude Number"
                };

                return response;
            }
           
            try
            {
                var result = _connectionManager.CreateCommand("submitswaprequest")
                               .WithSqlParam("msisdn", request.Msisdn)
                               .WithSqlParam("newsimserial", request.NewSimSerial)
                               .WithSqlParam("idtype", request.IdType)
                               .WithSqlParam("idnumber", request.IdNumber)
                               .WithSqlParam("reason", request.Reason)
                               .WithSqlParam("comments", request.Comments)
                               .WithSqlParam("idimage", request.IdImage.Replace(Environment.NewLine, "").Replace("\n", ""))
                               .WithSqlParam("requesterimage", request.RequesterImage.Replace(Environment.NewLine, "").Replace("\n", ""))
                               .WithSqlParam("longitude", request.Longitude)
                               .WithSqlParam("latitude", request.Latitude)
                               .WithSqlParam("userid", request.UserId, NpgsqlTypes.NpgsqlDbType.Bigint)
                               .WithSqlParam("fullname", request.Fullname)
                               .ExecuteReturningScalarAsync<int>();

                if (result.Result > 0)
                {
                    success = result.Result;
                }
            }
            catch (Exception ex)
            {
                _logger.logError(ex);
                return (new SubmitResponse
                {
                    Success = false,
                    Message = "Failed to Submit Sim Swap Request",
                    Exception = ex.ToString(),
                    SuccessID = success
                });
            }

            return (new SubmitResponse
            {
                Success = true,
                Message = "Request Has Successfully Been Saved",
                Exception = null,
                SuccessID = success
            });
        }

        private bool CheckAllDigits(string value)
        {
            if (value.Length != 12) {
                return false;
            }

            return value.ToCharArray().All(ch => ch >= 48 && ch <= 57);
        }

        private bool CheckCoordinates(string latitude, string longitude)
        {
            if(latitude == null || longitude == null)
            {
                return false;
            }

            if ((latitude.Replace("-", "").Replace(".", "").ToCharArray().All(ch => ch >= 48 && ch <= 57) && longitude.Replace("-", "").Replace(".", "").ToCharArray().All(ch => ch >= 48 && ch <= 57)) != true)
            {
                return false;
            }

            return true;
        }

        private ValidateId ValidateIdNumber(string IdType, string IdNumber)
        {
            if (IdType.ToLower() == "passport")
            {
                if (!Regex.IsMatch(IdNumber, @"[Gg]\d{7}|[Hh]\d{7}"))
                {
                    return new ValidateId
                    {
                        Success = false,
                        Message = "Invalid Passport"
                    };
                }

            }
            else if (IdType.ToLower() == "nhis")
            {
                if (!Regex.IsMatch(IdNumber, @"^\d{8}"))
                {
                    return new ValidateId
                    {
                        Success = false,
                        Message = "Invalid NHIS Card Number"
                    };
                }
            }
            else if (IdType.ToLower() == "voters")
            {
                if (!Regex.IsMatch(IdNumber, @"^\d{8}[A-Za-z]{2}$") && !Regex.IsMatch(IdNumber, @"^\d{10}$"))
                {
                    return new ValidateId
                    {
                        Success = false,
                        Message = "Invalid Voters' Card Number"
                    };
                }
            }
            else if (IdType.ToLower() == "national card")
            {
                if (!Regex.IsMatch(IdNumber, @"^[Cc]\d{12}|[Pp]\d{12}|[rR]\d{12}|[A-Za-z]{3}\-\d{9}\-\d{1}$"))
                {
                    return new ValidateId
                    {
                        Success = false,
                        Message = "Invalid National Card Number"
                    };
                }
            }
            else if (IdType.ToLower() == "driver's license")
            {
                if (!Regex.IsMatch(IdNumber, @"^[A-Za-z]{4}\d{10}|[A-Za-z]{4}-\d{6}-\d{2}-\d{2}|[A-Za-z]{3}-\d{8}-\d{5}|[A-Za-z]{3}\d{8}\d{5}$")) 
                {
                    return new ValidateId
                    {
                        Success = false,
                        Message = "Invalid Driver's ID Card Number"
                    };
                }
            }

            return new ValidateId
            {
                    Success = true,
                    Message = ""
            };
            
        }

        //private string SaveFileToDisk(string imageArray, bool isIDCard, string msisdn)
        //{
        //    //string folderName = AppDomain.CurrentDomain.BaseDirectory + @"Contents";
        //   string folderName = _hostingEnvironment.ContentRootPath + "\\Contents";

        //    string filePath = isIDCard ? Path.Combine(folderName, "IDCardImages") : Path.Combine(folderName, "RequesterImages");
        //    string fileName = (isIDCard ? "IdCardImageFor_" : "RequesterImageFor_") + msisdn + ".jpg";
        //    string fileFinalPath = Path.Combine(filePath, fileName);

        //    _logger.CheckDir(filePath);

        //    if (System.IO.File.Exists(fileFinalPath))
        //    {
        //        return fileFinalPath;
        //    }
        //    else
        //    {
        //        byte[] idcardimageArray;
        //        if (imageArray.StartsWith("data:image/jpeg;base64,"))
        //        {
        //            idcardimageArray = Convert.FromBase64String(imageArray.Replace("data:image/jpeg;base64,", string.Empty));
        //        }
        //        else
        //        {
        //            idcardimageArray = Convert.FromBase64String(imageArray);
        //        }

        //        Image image;

        //        using (MemoryStream ms = new MemoryStream(idcardimageArray))
        //        {
        //            image = Image.FromStream(ms);

        //            image.Save(fileFinalPath, ImageFormat.Jpeg);

        //        }

        //        return fileFinalPath;

        //        //var rootPathIdCard = "~/Contents/IDCardImages/{0}";
        //        //var rootPathPassport = "~/Contents/RequesterImages/{0}";
        //        //var _rootPathIdCard = "/Contents/IDCardImages/";
        //        //var _rootPathPassport = "/Contents/RequesterImages/";
        //        //string rootToUse = isIDCard ? _rootPathIdCard : _rootPathPassport;
        //        //string rootToReturn = isIDCard ? rootPathIdCard : rootPathPassport;

        //        //Image result = null;

        //        //ImageFormat format = ImageFormat.Png;

        //        //result = new Bitmap(new MemoryStream(idcardimageArray));

        //        //using (Image imageToExport = result)
        //        //{
        //        //   // string filepath = string.Format(fileFinalPath+".{0}", format.ToString());
        //        //    imageToExport.Save(fileFinalPath, format);
        //        //}

        //        //    //Image image = ;
        //        ////ms.Dispose();
        //        ////image.Save(rootPathIdCard);

        //        //System.IO.File.WriteAllBytes(fileFinalPath, idcardimageArray);

        //        //var filep = Url.Content(string.Format(rootToUse, fileName));
        //        //// var virtualpath = @"~\" + fileFinalPath.Replace(HttpContext.Current.Request.PhysicalApplicationPath, String.Empty);

        //        //return filep;
        //    }
        //}

    }
}
