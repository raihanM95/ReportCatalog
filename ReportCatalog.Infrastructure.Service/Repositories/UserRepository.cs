using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using ReportCatalog.Application.Interfaces;
using ReportCatalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ReportCatalog.Infrastructure.Service.Repositories
{
    public class UserRepository : IUserRepository<User>
    {
        private readonly IConfiguration _configuration;
        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IReadOnlyList<User>> GetAllAsync()
        {
            var query = "SELECT * FROM users";
            using (var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultDBConnection")))
            {
                connection.Open();
                var result = await connection.QueryAsync<User>(query);
                connection.Close();
                return result.ToList();
            }
        }

        public async Task<User> GetByNameAsync(string userName)
        {
            var query = "SELECT * FROM users WHERE username = @UserName";
            using (var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultDBConnection")))
            {
                connection.Open();
                var result = await connection.QuerySingleOrDefaultAsync<User>(query, new { UserName = userName });
                connection.Close();
                return result;
            }
        }

        public async Task<int> RegisterUserAsync(User entity)
        {
            var query = "INSERT INTO users (username,password,userType,createdBy,created) VALUES (@Username,@Password,@UserType,@CreatedBy,@Created)";
            using (var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultDBConnection")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(query, entity);
                connection.Close();
                return result;
            }
        }

        public async Task<User> GetUserAsync(string username, string password, string userType)
        {
            var query = "SELECT * FROM users WHERE username = @Username AND password = @Password AND userType = @UserType";
            using (var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultDBConnection")))
            {
                connection.Open();
                var result = await connection.QuerySingleOrDefaultAsync<User>(query, new { Username = username, Password = password, UserType = userType });
                connection.Close();
                return result;
            }
        }
    }
}
