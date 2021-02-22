using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FleetManagementSystem.Models
{
    // Database Model for Garage
    public class Garage
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Garage Name")]
        [Required]
        [MaxLength(75)]
        public string Name { get; set; }
        [MaxLength(200)]
        public string StreetAddress { get; set; }
        [MaxLength(50)]
        public string City { get; set; }
        [MaxLength(50)]
        public string State { get; set; }
        [MaxLength(25)]
        public string ZipCode { get; set; }
        //[Required(ErrorMessage = "You must provide a phone number")]
        //[Display(Name = "Home Phone")]
        //[DataType(DataType.PhoneNumber)]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        //public string PhoneNumber { get; set; }
        public int? ClosestGarageId { get; set; }
        [ForeignKey("ClosestGarageId")]
        public Garage ClosestGarage { get; set; }

    }
}
