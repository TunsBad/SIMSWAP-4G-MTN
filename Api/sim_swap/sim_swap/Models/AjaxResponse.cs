using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace sim_swap.Models
{
    public class AjaxResponse
    {
        public bool Success { get; set; }

        public string Exception { get; set; }

        public string Message { get; set; }

        public LoginResponse LoginData { get; set; }

        public VerifyResponse SubscriberDetails { get; set; }

        public IList<RequestResponse> SwapRequests { get; set; }


    }

    public class ParticularsResponse
    {
        public List<Image>ParticularImages { get; set; }
    }
}
