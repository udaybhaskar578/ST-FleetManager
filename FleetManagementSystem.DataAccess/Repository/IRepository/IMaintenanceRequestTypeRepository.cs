using FleetManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagementSystem.DataAccess.Repository.IRepository
{
    public interface IMaintenanceRequestTypeRepository : IRepository<MaintenanceRequestType>
    {
        void Update(MaintenanceRequestType maintenanceRequestType);
    }
}
