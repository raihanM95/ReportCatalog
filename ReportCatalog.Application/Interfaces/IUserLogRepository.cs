using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReportCatalog.Application.Interfaces
{
    public interface IUserLogRepository<UserLog>
    {
        Task<IReadOnlyList<UserLog>> GetAllAsync();
        Task<int> AddAsync(UserLog entity);
    }
}
