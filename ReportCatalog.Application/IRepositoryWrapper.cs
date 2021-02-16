using ReportCatalog.Application.Interfaces;
using ReportCatalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReportCatalog.Application
{
    public interface IRepositoryWrapper
    {
        IUserRepository<User> Users { get; }
        IUserLogRepository<UserLog> UserLogs { get; }
        ICategoryRepository Categories { get; }
        ISubCategoryRepository SubCategories { get; }
        IReportRepository Reports { get; }
    }
}
