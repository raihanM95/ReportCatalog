using ReportCatalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReportCatalog.Application.Interfaces
{
    public interface IUserRepository<User>
    {
        Task<User> GetByNameAsync(string userName);
        Task<int> RegisterUserAsync(User entity);
        Task<User> GetUserAsync(string userName, string password, string userType);
    }
}
