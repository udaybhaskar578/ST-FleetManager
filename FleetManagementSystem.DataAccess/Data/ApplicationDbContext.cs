using FleetManagementSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FleetManagementSystem.DataAccess.Data
{
    //Implements Identity DB context for the usage of ASP.Net Identity
    // Contains all the database tables that are required 
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Garage> Garages { get; set; }
        public DbSet<GarageAssignment> GarageAssignments { get; set; }
        public DbSet<MaintenanceRequestType> MaintenanceRequestTypes { get; set; }
        public DbSet<MaintenanceLineItem> MaintenanceLineItems { get; set; }
        public DbSet<Bus> Buses { get; set; }

    }
}
