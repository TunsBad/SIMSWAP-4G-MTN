using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace simswap_portal.Models
{
    public class AjaxResponse
    {
        public bool Success { get; set; }
        public int Result { get; set; }
    }

    public class SubscriberResponse
    {
        public bool Success { get; set; }
        public List<Subscriber> Subscribers { get; set; }
    }
}