using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagementSystem.Models
{
    // Database Model for Garage Assignment
    public class GarageAssignment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int BusId { get; set; }
        [Required]
        public int GarageId { get; set; }
        [Required]
        [Column(TypeName = "Date")]
        public DateTime CheckIn { get; set; }
        [Column(TypeName = "Date")]
        public DateTime? CheckOut { get; set; }
        [ForeignKey("BusId")]
        public Bus Bus { get; set; }
        [ForeignKey("GarageId")]
        public Garage Garage { get; set; }
    }
}
