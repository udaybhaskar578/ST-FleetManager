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
    public class BusRepository : Repository<Bus>, IBusRepository
    {
        private readonly ApplicationDbContext _db;
        public BusRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Bus bus)
        {
            _db.Update(bus);
        }
    }
}
