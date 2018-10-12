using System;

namespace sim_swap.Models
{
    public class LoginDetail
    {
        public Int32 UserId { get; set; }

        public string Msisdn { get; set; }

        public string Username { get; set; }

        public string UserPin { get; set; }

        public string Password { get; set; }

        public DateTime TimeStamp { get; set; }
    }

    public class PhoneLogin
    {
        public double Id { get; set; }

        public string Msisdn { get; set; }

        public string Pin { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ExpiryDate { get; set; }

    }

    public class LoginResponse
    {
        public Int32 UserId { get; set; }

        public String AuthToken { get; set; }

        public DateTime TokenExpiryDate { get; set; }

        public String Username { get; set; }
    }

}
