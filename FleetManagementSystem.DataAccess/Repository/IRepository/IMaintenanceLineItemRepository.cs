using FleetManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagementSystem.DataAccess.Repository.IRepository
{
    /*
    Abstract methods for the Maintenance Line Item entity
    */
    public interface IMaintenanceLineItemRepository : IRepository<MaintenanceLineItem>
    {
        void Update(MaintenanceLineItem maintenanceLineItem);
    }
}
