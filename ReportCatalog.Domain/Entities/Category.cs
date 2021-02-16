using ReportCatalog.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ReportCatalog.Domain.Entities
{
    public class Category : AuditableBaseEntity
    {
        [Required]
        public string Name { get; set; }
    }
}
