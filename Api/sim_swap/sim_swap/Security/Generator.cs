using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using sim_swap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace sim_swap.Security
{
    public class Generator: Controller
    {
        public IConfiguration _configuration;
        Integrator integrator;

        public Generator(IConfiguration configuration)
        {
            _configuration = configuration;
            integrator = new Integrator
            {
                IntegratorCode = configuration.GetValue<string>("IntegratorCode"),
                SecretCode = configuration.GetValue<string>("SecretCode")
            };
        }

        public string GetSHA1HashData(string data)
        {
            //create new instance of md5
            SHA1 sha1 = SHA1.Create();

            //convert the input text to array of bytes
            byte[] hashData = sha1.ComputeHash(Encoding.Default.GetBytes(data));

            //create new instance of StringBuilder to save hashed data
            StringBuilder returnValue = new StringBuilder();

            //loop for each byte and add it to StringBuilder
            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.AppendFormat("{0:x2}", hashData[i]);
            }

            // return hexadecimal string
            return returnValue.ToString();
        }

        public bool IsDateFutureDate(DateTime requestDate)
        {
            if (requestDate > DateTime.Now.AddMinutes(10))
            {
                return true;
            }
            return false;

        }
        public bool IsDateExpired(DateTime requestDate)
        {
            if (requestDate < DateTime.Now.AddHours(-1))
            {
                return true;
            }
            return false;

        }

        public class HashReponse
        {
            public string Hash { get; set; }
        }
    }
}
