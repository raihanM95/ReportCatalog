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
    public class SubCategoryRepository : ISubCategoryRepository
    {
        private readonly IConfiguration _configuration;
        public SubCategoryRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> AddAsync(SubCategory entity)
        {
            var query = "INSERT INTO sub_categories (name,categoryId,createdBy,created) VALUES (@Name,@CategoryId,@CreatedBy,@Created)";
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
            var query = "DELETE FROM sub_categories WHERE id = @Id";
            using (var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultDBConnection")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(query, new { Id = id });
                connection.Close();
                return result;
            }
        }

        public async Task<IReadOnlyList<SubCategory>> GetAllAsync()
        {
            var query = @"SELECT sub.id, sub.name, sub.createdBy, sub.created, sub.lastModifiedBy, sub.lastModified, cat.id AS categoryId, cat.name AS categoryName, pro.id AS projectId, pro.name AS projectName 
                          FROM sub_categories AS sub 
                          INNER JOIN categories AS cat 
                          ON sub.categoryId = cat.id 
                          INNER JOIN projects AS pro 
                          ON cat.projectId = pro.id";
            using (var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultDBConnection")))
            {
                connection.Open();
                var result = await connection.QueryAsync<SubCategory>(query);
                connection.Close();
                return result.ToList();
            }
        }

        public async Task<IReadOnlyList<SubCategory>> GetByCategoryIdAsync(int id)
        {
            var query = "SELECT * FROM sub_categories WHERE categoryId = @Id";
            using (var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultDBConnection")))
            {
                connection.Open();
                var result = await connection.QueryAsync<SubCategory>(query, new { Id = id });
                connection.Close();
                return result.ToList();
            }
        }

        public async Task<SubCategory> GetByIdAsync(int id)
        {
            var query = @"SELECT sub.id, sub.name, sub.createdBy, sub.created, sub.lastModifiedBy, sub.lastModified, cat.id AS categoryId, cat.name AS categoryName, pro.id AS projectId, pro.name AS projectName 
                          FROM sub_categories AS sub 
                          INNER JOIN categories AS cat 
                          ON sub.categoryId = cat.id 
                          INNER JOIN projects AS pro 
                          ON cat.projectId = pro.id WHERE sub.id = @Id";
            using (var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultDBConnection")))
            {
                connection.Open();
                var result = await connection.QuerySingleOrDefaultAsync<SubCategory>(query, new { Id = id });
                connection.Close();
                return result;
            }
        }

        public async Task<SubCategory> GetByNameAsync(string name)
        {
            var query = "SELECT * FROM sub_categories WHERE name = @Name";
            using (var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultDBConnection")))
            {
                connection.Open();
                var result = await connection.QuerySingleOrDefaultAsync<SubCategory>(query, new { Name = name });
                connection.Close();
                return result;
            }
        }

        public async Task<int> UpdateAsync(SubCategory entity)
        {
            var query = "UPDATE sub_categories SET name = @Name, lastModifiedBy = @LastModifiedBy, lastModified = @LastModified WHERE id = @Id";
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
