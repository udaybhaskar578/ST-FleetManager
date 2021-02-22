using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagementSystem.Models.ViewModels
{
    // Used in the Upsert page for Garage Assignments. Brings in all available Buses and Garages
    public class GarageAssignmentViewModel
    {
        public GarageAssignment GarageAssignment { get; set; }
        public IEnumerable<SelectListItem> BusList { get; set; }
        public IEnumerable<SelectListItem> GarageList { get; set; }
    }
}
