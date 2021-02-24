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
    public class ReportRepository : IReportRepository
    {
        private readonly IConfiguration _configuration;
        public ReportRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> AddAsync(Report entity)
        {
            var query = "INSERT INTO reports (name,fileName,storedProcedureName,description,inputImage,outputImage,subCategoryId,createdBy,created) VALUES (@Name,@FileName,@StoredProcedureName,@Description,@InputImage,@OutputImage,@SubCategoryId,@CreatedBy,@Created)";
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
            var query = "DELETE FROM reports WHERE id = @Id";
            using (var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultDBConnection")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(query, new { Id = id });
                connection.Close();
                return result;
            }
        }

        public async Task<IReadOnlyList<Report>> GetAllAsync()
        {
            var query = @"SELECT rep.id, rep.name, rep.fileName, rep.storedProcedureName, rep.description, rep.inputImage, rep.outputImage, rep.createdBy, rep.created, rep.lastModifiedBy, rep.lastModified, sub.id AS subCategoryId, sub.name AS subCategoryName, cat.id AS categoryId, cat.name AS categoryName, pro.id AS projectId, pro.name AS projectName
                          FROM reports AS rep 
                          INNER JOIN sub_categories AS sub 
                          ON rep.subCategoryId=sub.id 
                          INNER JOIN categories AS cat 
                          ON sub.categoryId=cat.id 
                          INNER JOIN projects AS pro 
                          ON cat.projectId = pro.id";
            using (var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultDBConnection")))
            {
                connection.Open();
                var result = await connection.QueryAsync<Report>(query);
                connection.Close();
                return result.ToList();
            }
        }

        public async Task<IReadOnlyList<Report>> GetBySubCategoryIdAsync(int id)
        {
            var query = "SELECT * FROM reports WHERE subCategoryId = @Id";
            using (var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultDBConnection")))
            {
                connection.Open();
                var result = await connection.QueryAsync<Report>(query, new { Id = id });
                connection.Close();
                return result.ToList();
            }
        }

        public async Task<Report> GetByIdAsync(int id)
        {
            var query = @"SELECT rep.id, rep.name, rep.fileName, rep.storedProcedureName, rep.description, rep.inputImage, rep.outputImage, rep.createdBy, rep.created, rep.lastModifiedBy, rep.lastModified, sub.id AS subCategoryId, sub.name AS subCategoryName, cat.id AS categoryId, cat.name AS categoryName, pro.id AS projectId, pro.name AS projectName
                          FROM reports AS rep 
                          INNER JOIN sub_categories AS sub 
                          ON rep.subCategoryId=sub.id 
                          INNER JOIN categories AS cat 
                          ON sub.categoryId=cat.id 
                          INNER JOIN projects AS pro 
                          ON cat.projectId = pro.id WHERE rep.id = @Id";
            using (var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultDBConnection")))
            {
                connection.Open();
                var result = await connection.QuerySingleOrDefaultAsync<Report>(query, new { Id = id });
                connection.Close();
                return result;
            }
        }

        public async Task<Report> GetByNameAsync(string name)
        {
            var query = "SELECT * FROM reports WHERE name = @Name";
            using (var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultDBConnection")))
            {
                connection.Open();
                var result = await connection.QuerySingleOrDefaultAsync<Report>(query, new { Name = name });
                connection.Close();
                return result;
            }
        }

        public async Task<int> UpdateAsync(Report entity)
        {
            var query = "UPDATE reports SET name = @Name, fileName = @FileName, lastModifiedBy = @LastModifiedBy, lastModified = @LastModified WHERE id = @Id";
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
