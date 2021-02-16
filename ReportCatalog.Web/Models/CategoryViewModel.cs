using ReportCatalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReportCatalog.Web.Models
{
    public class CategoryViewModel
    {
        public virtual int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Created { get; set; }

        public IEnumerable<Category> Categories { get; set; }
    }
}
