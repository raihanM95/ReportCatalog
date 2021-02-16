using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using ReportCatalog.Application.Interfaces;
using ReportCatalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportCatalog.Infrastructure.Service.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IConfiguration _configuration;
        public CategoryRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> AddAsync(Category entity)
        {
            var query = "INSERT INTO categories (name,createdBy,created) VALUES (@Name,@CreatedBy,@Created)";
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
            var query = "DELETE FROM categories WHERE id = @Id";
            using (var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultDBConnection")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(query, new { Id = id });
                connection.Close();
                return result;
            }
        }

        public async Task<IReadOnlyList<Category>> GetAllAsync()
        {
            var query = "SELECT * FROM categories";
            using (var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultDBConnection")))
            {
                connection.Open();
                var result = await connection.QueryAsync<Category>(query);
                connection.Close();
                return result.ToList();
            }
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM categories WHERE id = @Id";
            using (var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultDBConnection")))
            {
                connection.Open();
                var result = await connection.QuerySingleOrDefaultAsync<Category>(query, new { Id = id });
                connection.Close();
                return result;
            }
        }

        public async Task<Category> GetByNameAsync(string name)
        {
            var query = "SELECT * FROM categories WHERE name = @Name";
            using (var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultDBConnection")))
            {
                connection.Open();
                var result = await connection.QuerySingleOrDefaultAsync<Category>(query, new { Name = name });
                connection.Close();
                return result;
            }
        }

        public async Task<int> UpdateAsync(Category entity)
        {
            var query = "UPDATE categories SET name = @Name, lastModifiedBy = @LastModifiedBy, lastModified = @LastModified WHERE id = @Id";
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
