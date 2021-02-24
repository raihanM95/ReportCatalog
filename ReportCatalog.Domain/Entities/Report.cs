using ReportCatalog.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ReportCatalog.Domain.Entities
{
    public class Report : AuditableBaseEntity
    {
        [Required]
        public string Name { get; set; }
        public string FileName { get; set; }
        public string StoredProcedureName { get; set; }
        public string Description { get; set; }
        [Required]
        public string InputImage { get; set; }
        [Required]
        public string OutputImage { get; set; }
        //[Required]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        [Required]
        public int SubCategoryId { get; set; }

        public string SubCategoryName { get; set; }
        [Required]
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
    }
}
