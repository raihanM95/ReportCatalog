using ReportCatalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReportCatalog.Application.Interfaces
{
    public interface IReportRepository : IRepositoryBase<Report>
    {
        Task<IReadOnlyList<Report>> GetBySubCategoryIdAsync(int id);
    }
}
