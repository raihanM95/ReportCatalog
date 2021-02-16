using ReportCatalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReportCatalog.Application.Interfaces
{
    public interface ISubCategoryRepository : IRepositoryBase<SubCategory>
    {
        Task<IReadOnlyList<SubCategory>> GetByCategoryIdAsync(int id);
    }
}
