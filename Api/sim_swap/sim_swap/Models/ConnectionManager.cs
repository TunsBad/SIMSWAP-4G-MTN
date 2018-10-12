using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace sim_swap.Models
{
    public class ConnectionManager
    {
        private readonly IConfiguration _configuration;

        public ConnectionManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public NpgsqlCommand CreateCommand(string commandName)
        {
            var connection = new NpgsqlConnection(_configuration.GetConnectionString("Database"));
            var command = new NpgsqlCommand(commandName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            return command;
        }

        public NpgsqlCommand CreateSmsCommand(string commandName)
        {
            var connection = new NpgsqlConnection(_configuration.GetValue<string>("smppDbConStrg"));
            var command = new NpgsqlCommand(commandName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            return command;
        }
    }
}
