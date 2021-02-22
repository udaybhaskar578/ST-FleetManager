using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagementSystem.Models
{
    public class MaintenanceRequestType
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Maintenance Request Type")]
        [Required]
        [MaxLength(75)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        [Range(1, 10000)]
        public double ListPrice { get; set; }

        public bool IsActive { get; set; } = true;

    }
}
