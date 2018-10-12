using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using sim_swap.Models;

namespace sim_swap.Security
{
    public class Authorization: Controller
    {
        public IConfiguration _configuration;
        public Generator _generator;
        Integrator integrator;

        public Authorization(IConfiguration configuration, Generator generator)
        {
            _configuration = configuration;
            _generator = generator;

            integrator = new Integrator
            {
                IntegratorCode = configuration.GetValue<string>("IntegratorCode"),
                SecretCode = configuration.GetValue<string>("SecretCode")
            };
        }

        [HttpPost]
        public bool AuthenticateRequest(DateTime requestTimestamp, HttpRequest req)
        {
            // var authHash = req.Headers.Authorization.Parameter;
            req.Headers.TryGetValue("Authorization", out Microsoft.Extensions.Primitives.StringValues authHash);

            // var integratorCode = req.Headers.Authorization.Scheme;
            req.Headers.TryGetValue("IntegratorCode", out Microsoft.Extensions.Primitives.StringValues integratorCode);

            if (_generator.IsDateFutureDate(requestTimestamp))
            {
                throw new ApplicationException("Request date cannot be a future date");
            }
            if (_generator.IsDateExpired(requestTimestamp))
            {
                throw new ApplicationException("Request date expired");
            }

            if (integrator.IntegratorCode != integratorCode)
                return false;

            var strDate = requestTimestamp.ToString("yyyyMMddHHmmss");
            var hash = _generator.GetSHA1HashData(strDate + integrator.SecretCode);
            authHash = _generator.GetSHA1HashData(authHash);

            if (authHash == hash)
            {
                return true;
            }
            return false;
        }
    }
}
