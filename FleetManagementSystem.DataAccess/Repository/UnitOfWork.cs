using FleetManagementSystem.DataAccess.Data;
using FleetManagementSystem.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

/*
The repository and unit of work patterns are intended to create an abstraction layer 
between the data access layer and the business logic layer of an application. 
Implementing these patterns can help insulate your application from changes 
in the data store and can facilitate automated unit testing or test-driven development
*/
namespace FleetManagementSystem.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public IGarageRepository Garage { get; private set; }
        public IGarageAssignmentRepository GarageAssignment { get; private set; }
        public IMaintenanceRequestTypeRepository MaintenanceRequestType { get; private set; }
        public IMaintenanceLineItemRepository MaintenanceLineItem { get; private set; }
        public IBusRepository Bus { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            _db.ChangeTracker.Clear();
            ApplicationUser = new ApplicationUserRepository(_db);
            Garage = new GarageRepository(_db);
            GarageAssignment = new GarageAssignmentRepository(_db);
            MaintenanceRequestType = new MaintenanceRequestTypeRepository(_db);
            MaintenanceLineItem = new MaintenanceLineItemRepository(_db);
            Bus = new BusRepository(_db);
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {

            _db.SaveChanges();
            _db.ChangeTracker.Clear();


        }
    }
}
