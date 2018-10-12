using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using simswap_portal.Models;
using simswap_portal.Utility;

namespace simswap_portal.API
{
    public class SimSwapController : ApiController
    {
        DBHelper _dbhelper = new DBHelper();

        [Route("api/simswap/HighValueSubscribers")]
        [HttpPost]
        public KendoResponse HighValueSubscribers([FromBody] KendoRequest req)
        {
            List<Subscriber> highvaluesubscribers = new List<Subscriber>();

            var order = "Id";
            KendoHelper.getSortOrder(req, ref order);
            var parameters = new List<object>();
            var whereClause = KendoHelper.getWhereClause<Subscriber>(req, parameters);

            highvaluesubscribers = _dbhelper.HighValueSubscribers();

            var query = highvaluesubscribers.OrderBy(p => p.Id).AsQueryable();

            if (req != null && req.sort != null && req.sort.Any())
            {
                var sort = req.sort.First();
                if (sort.dir != "asc")
                {

                }
            }

            var data = query
                .Skip(req.skip)
                .Take(req.take)
                .ToArray();

            return new KendoResponse(data, query.Count());
        }

        [Route("api/simswap/CreateHighValueSubscriber")]
        [HttpPost]
        public AjaxResponse CreateHighValueSubscriber(Subscriber subscriber)
        {
            int result = 0;
            try
            {
                result = _dbhelper.CreateHighValueSubscriber(subscriber.Msisdn);
                if (result != 0)
                {
                    return new AjaxResponse
                    {
                        Success = true,
                        Result = result
                    };
                }
                else if (result == 0)
                {
                    return new AjaxResponse
                    {
                        Success = false,
                        Result = result
                    };
                }

            }
            catch (Exception ex)
            {
                Console.Write(ex);

            }

            return new AjaxResponse
            {
                Success = true,
                Result = result
            };
        }

        [Route("api/simswap/DeleteHighValueSubscriber")]
        [HttpPost]
        public AjaxResponse DeleteHighValueSubscriber(Subscriber subscriber)
        {
            int result = 0;

            try {
                result = _dbhelper.DestroyHighValueSubscriber(subscriber.Id);

                return new AjaxResponse
                {
                    Success= true,
                    Result = result
                };

            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }

            return new AjaxResponse
            {
                Success = true,
                Result = result
            };

        }

        [Route("api/simswap/UpdateHighValueSubscriber")]
        [HttpPost]
        public AjaxResponse UpdateHighValueSubscriber(Subscriber subscriber)
        {
            int result = 0;
            try
            {
                result = _dbhelper.UpdateHighValueSubscriber(subscriber);
                if (result == 1)
                {
                    return new AjaxResponse
                    {
                        Success = true,
                        Result = result
                    };
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return new AjaxResponse
            {
                Success = true,
                Result = result
            };
        }

        [Route("api/simswap/UpdateThreshold")]
        [HttpPost]
        public Threshold UpdateThreshold(int threshold)
        {
            Threshold thres = new Threshold { };
            int success = 0;
            try
            {
                success = _dbhelper.UpdateThreshold(threshold);
                thres.ThresholdValue = success;
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }

            return thres;
        }

        [Route("api/simswap/GetAllSimSwapRequests")]
        [HttpPost]
        public KendoResponse GetAllSimSwapRequests([FromBody] KendoRequest req)
        {
            List<SimSwapRequests> swaprequests = new List<SimSwapRequests>();

            var order = "DateSubmitted";
            KendoHelper.getSortOrder(req, ref order);
            var parameters = new List<object>();
            var whereClause = KendoHelper.getWhereClause<Subscriber>(req, parameters);

            swaprequests = _dbhelper.GetAllSimSwapRequests();

            var query = swaprequests.OrderBy(p => p.DateSubmitted).AsQueryable();

            if (req != null && req.sort != null && req.sort.Any())
            {
                var sort = req.sort.First();
                if (sort.dir != "asc")
                {

                }
            }

            var data = query
                .Skip(req.skip)
                .Take(req.take)
                .ToArray();

            return new KendoResponse(data, query.Count());
        }


        [Route("api/simswap/GetSimSwapRequestsByUser")]
        [HttpPost]
        public KendoResponse GetSimSwapRequestsByUser([FromBody] KendoRequest req, int userid)
        {
            List<SimSwapRequests> swaprequests = new List<SimSwapRequests>();

            var order = "Id";
            KendoHelper.getSortOrder(req, ref order);
            var parameters = new List<object>();
            var whereClause = KendoHelper.getWhereClause<Subscriber>(req, parameters);

            swaprequests = _dbhelper.GetSimSwapRequestByUser(userid);

            var query = swaprequests.OrderBy(p => p.Id).AsQueryable();

            if (req != null && req.sort != null && req.sort.Any())
            {
                var sort = req.sort.First();
                if (sort.dir != "asc")
                {

                }
            }

            var data = query
                .Skip(req.skip)
                .Take(req.take)
                .ToArray();

            return new KendoResponse(data, query.Count());
        }

        [Route("api/simswap/GetAllUsers")]
        [HttpPost]
        public KendoResponse GetAllUsers([FromBody] KendoRequest req)
        {
            List<User> users = new List<User> { };

            var order = "UserId";
            KendoHelper.getSortOrder(req, ref order);
            var parameters = new List<object>();
            var whereClause = KendoHelper.getWhereClause<Subscriber>(req, parameters);

            users = _dbhelper.GetUsers();

            var query = users.OrderBy(p => p.UserId).AsQueryable();

            if (req != null && req.sort != null && req.sort.Any())
            {
                var sort = req.sort.First();
                if (sort.dir != "asc")
                {

                }
            }

            var data = query
                .Skip(req.skip)
                .Take(req.take)
                .ToArray();

            return new KendoResponse(data, query.Count());
        }

        [Route("api/simswap/GetRequestById")]
        [HttpGet]
        public List<Request> GetRequestById(int id) {

            List<Request> detailsForEditing = new List<Request> { };

            detailsForEditing = _dbhelper.GetRequestById(id);

            return detailsForEditing;
            
        }

        [Route("api/simswap/GetRequestDetailsById")]
        [HttpGet]
        public List<RequestDetail> GetRequestDetailsById(int requestid)
        {

            List<RequestDetail> requestDetails = new List<RequestDetail> { };

            requestDetails = _dbhelper.GetRequestDetailsById(requestid);

            return requestDetails;

        }


        [Route("api/simswap/EditRequest")]
        [HttpPost]
        public int EditRequest([FromBody]Request editedrequest)
        {
            var result = _dbhelper.UpdateRequest(editedrequest);

            return result;
        }


        [Route("api/simswap/GetPendingRequests")]
        [HttpPost]
        public KendoResponse GetPendingRequests([FromBody] KendoRequest req)
        {
            List<SimSwapRequests> swaprequests = new List<SimSwapRequests>();

            var order = "DateSubmitted";
            KendoHelper.getSortOrder(req, ref order);
            var parameters = new List<object>();
            var whereClause = KendoHelper.getWhereClause<Subscriber>(req, parameters);

            swaprequests = _dbhelper.GetPendingRequests();

            var query = swaprequests.OrderBy(p => p.Id).AsQueryable();

            if (req != null && req.sort != null && req.sort.Any())
            {
                var sort = req.sort.First();
                if (sort.dir != "asc")
                {

                }
            }

            var data = query
                .Skip(req.skip)
                .Take(req.take)
                .ToArray();

            return new KendoResponse(data, query.Count());
        }

        [Route("api/simswap/GetFulfilledRequests")]
        [HttpPost]
        public KendoResponse GetFulfilledRequests([FromBody] KendoRequest req)
        {
            List<SimSwapRequests> swaprequests = new List<SimSwapRequests>();

            var order = "DateSubmitted";
            KendoHelper.getSortOrder(req, ref order);
            var parameters = new List<object>();
            var whereClause = KendoHelper.getWhereClause<Subscriber>(req, parameters);

            swaprequests = _dbhelper.GetFulfilledRequests();

            var query = swaprequests.OrderBy(p => p.Id).AsQueryable();

            if (req != null && req.sort != null && req.sort.Any())
            {
                var sort = req.sort.First();
                if (sort.dir != "asc")
                {

                }
            }

            var data = query
                .Skip(req.skip)
                .Take(req.take)
                .ToArray();

            return new KendoResponse(data, query.Count());
        }


        [Route("api/simswap/UserProfilePhoto")]
        [HttpGet]
        public HttpResponseMessage UserProfilePhoto(int requestid)
        {
            var photoContentString = "FIRST_PHOTO_AWAITING_DB_BASE64";
            photoContentString = _dbhelper.CustomerImage(requestid);

            if (string.IsNullOrEmpty(photoContentString))
            {
                photoContentString = "NO_PHOTO_PLACEHOLDER_BASE64";
            }

            byte[] photoContentArray;

            if (photoContentString.StartsWith("data:image/jpeg;base64,"))
            {
                try
                {
                    photoContentArray = Convert.FromBase64String(photoContentString.Replace("data:image/jpeg;base64,", string.Empty));
                }
                catch (Exception ex)
                {
                    photoContentString = "NO_PHOTO_PLACEHOLDER_BASE64";
                    photoContentArray = Convert.FromBase64String(photoContentString);
                    Console.Write(ex);
                }
            }
            else
            {
                try
                {
                    photoContentArray = Convert.FromBase64String(photoContentString);
                }
                catch (Exception ex)
                {
                    photoContentString = "NO_PHOTO_PLACEHOLDER_BASE64";
                    photoContentArray = Convert.FromBase64String(photoContentString);
                    Console.Write(ex);
                }
            }

            HttpResponseMessage message = new HttpResponseMessage();
            message.Content = new ByteArrayContent(photoContentArray);
            message.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
            message.StatusCode = HttpStatusCode.OK;

            return message;

        }

        [Route("api/simswap/UserIdcardPhoto")]
        [HttpGet]
        public HttpResponseMessage UserIdcardPhoto(int requestid)
        {
            var photoContentString = "FIRST_PHOTO_AWAITING_DB_BASE64";
            photoContentString = _dbhelper.CustomerIdcardImage(requestid);

            if (string.IsNullOrEmpty(photoContentString))
            {
                photoContentString = "NO_PHOTO_PLACEHOLDER_BASE64";
            }

            byte[] photoContentArray;

            if (photoContentString.StartsWith("data:image/jpeg;base64,"))
            {
                try
                {
                    photoContentArray = Convert.FromBase64String(photoContentString.Replace("data:image/jpeg;base64,", string.Empty));
                }
                catch (Exception ex)
                {
                    photoContentString = "NO_PHOTO_PLACEHOLDER_BASE64";
                    photoContentArray = Convert.FromBase64String(photoContentString);
                    Console.Write(ex);
                }
            }
            else
            {
                try
                {
                    photoContentArray = Convert.FromBase64String(photoContentString);
                }
                catch (Exception ex)
                {
                    photoContentString = "NO_PHOTO_PLACEHOLDER_BASE64";
                    photoContentArray = Convert.FromBase64String(photoContentString);
                    Console.Write(ex);
                }
            }

            HttpResponseMessage message = new HttpResponseMessage();
            message.Content = new ByteArrayContent(photoContentArray);
            message.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
            message.StatusCode = HttpStatusCode.OK;

            return message;

        }

        [Route("api/simswap/RequesterHistory")]
        [HttpPost]
        public KendoResponse RequesterHistory([FromBody] KendoRequest req, string msisdn)
        {

            List<Request> swaprequests = new List<Request>();

            var order = "DateSubmitted";
            KendoHelper.getSortOrder(req, ref order);
            var parameters = new List<object>();
            var whereClause = KendoHelper.getWhereClause<Subscriber>(req, parameters);

            swaprequests = _dbhelper.GetRequesterHistory(msisdn);

            var query = swaprequests.OrderBy(p => p.Id).AsQueryable();

            if (req != null && req.sort != null && req.sort.Any())
            {
                var sort = req.sort.First();
                if (sort.dir != "asc")
                {

                }
            }

            var data = query
                .Skip(req.skip)
                .Take(req.take)
                .ToArray();

            return new KendoResponse(data, query.Count());
        }

        [Route("api/simswap/CheckSubscriber")]
        [HttpGet]
        public int CheckSubscriber(string cellid)
        {
            var result = DBHelper.Instance.CheckSubscriber(cellid);
            return result;
        }

        [Route("api/simswap/CheckAgent")]
        [HttpGet]
        public int CheckAgent(string phonenumber)
        {
            var result = DBHelper.Instance.CheckAgent(phonenumber);
            return result;
        }

    }
}
