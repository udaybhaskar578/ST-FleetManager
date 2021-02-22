using FleetManagementSystem.DataAccess.Data;
using FleetManagementSystem.DataAccess.Repository.IRepository;
using FleetManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FleetManagementSystem.DataAccess.Repository
{
    /*
    DBMethod Implementations that are Specific to Maintenance Request Type
    */
    public class MaintenanceRequestTypeRepository : Repository<MaintenanceRequestType>, IMaintenanceRequestTypeRepository
    {
        private readonly ApplicationDbContext _db;
        public MaintenanceRequestTypeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(MaintenanceRequestType maintenanceRequestType)
        {
            _db.ChangeTracker.Clear();

            _db.Update(maintenanceRequestType);
        }
    }
}
