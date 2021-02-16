using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using ReportCatalog.Application.Interfaces;
using ReportCatalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportCatalog.Infrastructure.Service.Repositories
{
    public class UserLogRepository : IUserLogRepository<UserLog>
    {
        private readonly IConfiguration _configuration;
        public UserLogRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> AddAsync(UserLog entity)
        {
            var query = "INSERT INTO user_log (username,userType,time,type,ip) VALUES (@Username,@UserType,@Time,@Type,@IP)";
            using (var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultDBConnection")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(query, entity);
                connection.Close();
                return result;
            }
        }

        public async Task<IReadOnlyList<UserLog>> GetAllAsync()
        {
            var query = "SELECT * FROM user_log AS log ORDER BY log.time DESC";
            using (var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultDBConnection")))
            {
                connection.Open();
                var result = await connection.QueryAsync<UserLog>(query);
                connection.Close();
                return result.ToList();
            }
        }
    }
}
