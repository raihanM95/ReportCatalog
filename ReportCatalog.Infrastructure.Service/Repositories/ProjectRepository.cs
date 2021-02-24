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
    public class ProjectRepository : IProjectRepository
    {
        private readonly IConfiguration _configuration;
        public ProjectRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> AddAsync(Project entity)
        {
            var query = "INSERT INTO projects (name,createdBy,created) VALUES (@Name,@CreatedBy,@Created)";
            using (var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultDBConnection")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(query, entity);
                connection.Close();
                return result;
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            var query = "DELETE FROM projects WHERE id = @Id";
            using (var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultDBConnection")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(query, new { Id = id });
                connection.Close();
                return result;
            }
        }

        public async Task<IReadOnlyList<Project>> GetAllAsync()
        {
            var query = "SELECT * FROM projects";
            using (var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultDBConnection")))
            {
                connection.Open();
                var result = await connection.QueryAsync<Project>(query);
                connection.Close();
                return result.ToList();
            }
        }

        public async Task<Project> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM projects WHERE id = @Id";
            using (var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultDBConnection")))
            {
                connection.Open();
                var result = await connection.QuerySingleOrDefaultAsync<Project>(query, new { Id = id });
                connection.Close();
                return result;
            }
        }

        public async Task<Project> GetByNameAsync(string name)
        {
            var query = "SELECT * FROM projects WHERE name = @Name";
            using (var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultDBConnection")))
            {
                connection.Open();
                var result = await connection.QuerySingleOrDefaultAsync<Project>(query, new { Name = name });
                connection.Close();
                return result;
            }
        }

        public async Task<int> UpdateAsync(Project entity)
        {
            var query = "UPDATE projects SET name = @Name, lastModifiedBy = @LastModifiedBy, lastModified = @LastModified WHERE id = @Id";
            using (var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultDBConnection")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(query, entity);
                connection.Close();
                return result;
            }
        }
    }
}
