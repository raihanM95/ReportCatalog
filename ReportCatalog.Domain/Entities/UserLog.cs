using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ReportCatalog.Domain.Entities
{
    public class UserLog
    {
        public virtual int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string UserType { get; set; }
        [Required]
        public DateTime? Time { get; set; }
        [Required]
        public string Type { get; set; }
        public string IP { get; set; }
    }
}
