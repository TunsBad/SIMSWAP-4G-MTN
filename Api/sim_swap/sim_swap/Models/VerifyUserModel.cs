using System;

namespace sim_swap.Models
{
    public class VerifyNumber
    {
        public string PhoneNumber { get; set; }
    }

    public class VerifyResponse
    {
        public string Msisdn { get; set; }

        public string FullName { get; set; }

        public DateTime Dob { get; set; }

        public string IdType { get; set; }

        public string IdNumber { get; set; }

        public long HighValueId { get; set; }
    }
}
