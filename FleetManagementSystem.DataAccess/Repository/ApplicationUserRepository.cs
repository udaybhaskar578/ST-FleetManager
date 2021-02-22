using FleetManagementSystem.DataAccess.Data;
using FleetManagementSystem.DataAccess.Repository.IRepository;
using FleetManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FleetManagementSystem.DataAccess.Repository
{
    /*
    DBMethod Implementations that are Specific to User
    */
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDbContext _db;
        public ApplicationUserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

    }
}
