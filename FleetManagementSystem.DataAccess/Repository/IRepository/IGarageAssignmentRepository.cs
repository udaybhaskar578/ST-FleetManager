using FleetManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagementSystem.DataAccess.Repository.IRepository
{
    /*
    Abstract methods for the Garage Assignment entity
    */
    public interface IGarageAssignmentRepository: IRepository<GarageAssignment>
    {
        void Update(GarageAssignment garageAssignment);
    }
}
