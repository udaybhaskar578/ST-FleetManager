using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagementSystem.Models
{
    public class MaintenanceLineItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int BusId { get; set; }
        [Required]
        public int GarageId { get; set; } 
        [Required]
        public int MaintenanceRequestTypeId { get; set; }
        [ForeignKey("BusId")]
        public Bus Bus { get; set; }
        [ForeignKey("GarageId")]
        public Garage Garage { get; set; }
        [ForeignKey("MaintenanceRequestTypeId")]
        public MaintenanceRequestType MaintenanceRequestType { get; set; }
        [Required]
        [Column(TypeName = "Date")]
        public DateTime RequestRaisedOn { get; set; }
        [Column(TypeName = "Date")]
        public DateTime? CompletedOn { get; set; }
        public string Status { get; set; }
    }
}
