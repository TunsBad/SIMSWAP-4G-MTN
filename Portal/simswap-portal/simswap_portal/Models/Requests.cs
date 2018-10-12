using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace simswap_portal.Models
{
    public class SimSwapRequests
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public string Msisdn { get; set; }
        public string NewSimSerial { get; set; }
        public string IdType { get; set; }
        public string IdNumber { get; set; }
        public string Reason { get; set; }
        public string Comments { get; set; }
        public int LocationId { get; set; }
        public int UserId { get; set; }
        public int AttachmentId { get; set; }
        public DateTime DateSubmitted { get; set; }
        public string Status { get; set; }
    }

    public class Request
    {
        public int Id { get; set; }
        public string Msisdn { get; set; }
        public string SimSerial { get; set; }
        public string IdType { get; set; }
        public string IdNumber { get; set; }
        public string Reason { get; set; }
        public string Comment { get; set; }
        public string Status { get; set; }
        public DateTime DateSubmitted { get; set; }
    }


    public class RequestDetail
    {
        public int Id { get; set; }
        public string Msisdn { get; set; }
        public string SimSerial { get; set; }
        public string IdType { get; set; }
        public string IdNumber { get; set; }
        public string Reason { get; set; }
        public string Comment { get; set; }
        public DateTime DateSubmitted { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string Status { get; set; }
    }

}