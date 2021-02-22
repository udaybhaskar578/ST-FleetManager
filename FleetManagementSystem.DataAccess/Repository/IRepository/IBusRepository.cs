using FleetManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagementSystem.DataAccess.Repository.IRepository
{
    /*
    Abstract methods for the Bus entity
    */
    public interface IBusRepository : IRepository<Bus>
    {
        void Update(Bus bus);
    }
}
