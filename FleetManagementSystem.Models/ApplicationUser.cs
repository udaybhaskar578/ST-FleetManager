using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FleetManagementSystem.Models
{
    /*
    Application user implements the Identity User in ASP.Net.Identity
    This allows us to build the user infra on top of the existing  ASPNetUser and roles
    */
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        [NotMapped]
        public string Role { get; set; }

    }
}
