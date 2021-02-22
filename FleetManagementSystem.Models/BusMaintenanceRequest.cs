using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagementSystem.Models
{
    // Database Model for Bus Maintenance Request
    public class BusMaintenanceRequest
    {
        [Key]
        public int ServiceLogId { get; set; }
        [Required]
        public int BusId { get; set; }
        [ForeignKey("BusId")]
        public Bus Bus { get; set; }
    }
}
