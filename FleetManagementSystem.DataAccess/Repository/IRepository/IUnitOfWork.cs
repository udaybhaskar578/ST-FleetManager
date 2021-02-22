using System;
using System.Collections.Generic;
using System.Text;

namespace FleetManagementSystem.DataAccess.Repository.IRepository
{
    /*
    Abstract methods and delarations for the UnitOfWork
    */
    public interface IUnitOfWork : IDisposable
    {
        IApplicationUserRepository ApplicationUser { get; }
        IGarageRepository Garage { get; } 
        IGarageAssignmentRepository GarageAssignment { get; }
        IMaintenanceRequestTypeRepository MaintenanceRequestType { get; }
        IMaintenanceLineItemRepository MaintenanceLineItem { get; }
        IBusRepository Bus { get; }
        void Save();
    }
}
