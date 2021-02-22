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
    public class MaintenanceLineItemRepository : Repository<MaintenanceLineItem>, IMaintenanceLineItemRepository
    {
        private readonly ApplicationDbContext _db;
        public MaintenanceLineItemRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(MaintenanceLineItem maintenanceLineItem)
        {
            _db.Update(maintenanceLineItem);
        }
    }
}
