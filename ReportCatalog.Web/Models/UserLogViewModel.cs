using ReportCatalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportCatalog.Web.Models
{
    public class UserLogViewModel
    {
        public IEnumerable<UserLog> UserLogs { get; set; }
    }
}
