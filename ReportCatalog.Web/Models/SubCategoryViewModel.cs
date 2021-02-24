using ReportCatalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReportCatalog.Web.Models
{
    public class SubCategoryViewModel
    {
        public virtual int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Created { get; set; }
        [Required]
        public int ProjectId { get; set; }
        [Required]
        public int CategoryId { get; set; }

        public IEnumerable<SubCategory> SubCategories { get; set; }
    }
}
