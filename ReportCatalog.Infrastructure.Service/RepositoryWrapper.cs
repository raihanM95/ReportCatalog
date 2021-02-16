using ReportCatalog.Application;
using ReportCatalog.Application.Interfaces;
using ReportCatalog.Domain.Entities;
using ReportCatalog.Infrastructure.Service.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReportCatalog.Infrastructure.Service
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        public IUserRepository<User> Users { get; }
        public IUserLogRepository<UserLog> UserLogs { get; }
        public ICategoryRepository Categories { get; }
        public ISubCategoryRepository SubCategories { get; }
        public IReportRepository Reports { get; }

        public RepositoryWrapper(
            IUserRepository<User> users,
            IUserLogRepository<UserLog> userLog,
            ICategoryRepository categories,
            ISubCategoryRepository subCategories,
            IReportRepository reports)
        {
            Users = users;
            UserLogs = userLog;
            Categories = categories;
            SubCategories = subCategories;
            Reports = reports;
        }
    }
}
