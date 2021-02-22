using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FleetManagementSystem.Models
{
    // Database Model for Bus
    public class Bus
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [Range(1830, 2021)]
        public int Year { get; set; }

        [Required]
        [Range(4, 12)]
        public int NumberOfWheels { get; set; }
        [Required]
        public int MaximumCapacity { get; set; }
        [Required]
        public bool AirConditioning { get; set; }
        [Required]
        public string CurrentStatus { get; set; }
        [Required]
        public string VIN { get; set; }
        [Required]
        public string RegistrationNumber { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }

        public double OdometerReading { get; set; }
        public string ImageUrl { get; set; }
        public double ResaleValue { get; set; }


    }
}
