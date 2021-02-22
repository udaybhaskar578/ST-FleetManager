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
    DBMethod Implementations that are Specific to Garage
    */
    public class GarageRepository : Repository<Garage>, IGarageRepository
    {
        private readonly ApplicationDbContext _db;
        public GarageRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Garage garage)
        {
            var objFromDb = _db.Garages.FirstOrDefault(s => s.Id == garage.Id);
            if (objFromDb != null)
            {

                objFromDb.Name = garage.Name;
                objFromDb.StreetAddress = garage.StreetAddress;
                objFromDb.City = garage.City;
                objFromDb.State = garage.State;
                objFromDb.ZipCode = garage.ZipCode;
                objFromDb.ClosestGarageId = garage.ClosestGarageId;
                _db.ChangeTracker.Clear();
                _db.Update(objFromDb);

            }
        }
    }
}
