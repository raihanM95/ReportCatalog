using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReportCatalog.Application;
using ReportCatalog.Application.Interfaces;
using ReportCatalog.Domain.Entities;
using ReportCatalog.Infrastructure.Service;
using ReportCatalog.Infrastructure.Service.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportCatalog.Web.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
            services.AddScoped<IUserRepository<User>, UserRepository>();
            services.AddScoped<IUserLogRepository<UserLog>, UserLogRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ISubCategoryRepository, SubCategoryRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();

        }

        public static void ConfigureSession(this IServiceCollection services)
        {
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(7);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
        }
    }
}
