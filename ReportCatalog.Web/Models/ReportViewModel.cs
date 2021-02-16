using Microsoft.AspNetCore.Http;
using ReportCatalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReportCatalog.Web.Models
{
    public class ReportViewModel
    {
        public virtual int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string StoredProcedureName { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Please choose input image")]
        public IFormFile InputImage { get; set; }
        [Required(ErrorMessage = "Please choose output image")]
        public IFormFile OutputImage { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int SubCategoryId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Created { get; set; }

        public IEnumerable<Report> Reports { get; set; }
    }
}
