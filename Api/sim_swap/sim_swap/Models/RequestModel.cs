using Microsoft.AspNetCore.Mvc;
using sim_swap.Controllers;
using System.Collections.Generic;

namespace sim_swap.Models
{
    public class RequestModel
    {
        public int Id { get; set; }
        public string Msisdn { get; set; }
        public string NewSimSerial { get; set; }
        public string IdType { get; set; }
        public string IdNumber { get; set; }
        public string Reason { get; set; }
        public string Comments { get; set; }
        public string IdImage { get; set; }
        public string RequesterImage { get; set; }
        public int UserId { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string Fullname { get; set; }
    }

    public class RequestResponse
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public string Msisdn { get; set; }
        public string NewSimSerial { get; set; }
        public string IdType { get; set; }
        public string IdNumber { get; set; }
        public string Reason { get; set; }
        public string Comments { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }

        public Attachment Attachment { get; set; }
    }

    public class SubmitResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public int SuccessID { get; set; }
    }

    public class Attachment
    {
        public FileDetail IdCardImage { get; set; }
        public FileDetail RequesterImage { get; set; }
    }

    public class AttachmentResponse
    {
        public string IdCardImage { get; set; }
        public string RequesterImage { get; set; }
    }

    public class FileDetail
    {
        public string FileName { get; set; }
        public string FileUrl { get; set; }
    }

    public class ValidateId
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
