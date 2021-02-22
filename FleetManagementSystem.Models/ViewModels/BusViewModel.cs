using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagementSystem.Models.ViewModels
{
    //Bus view Model is used on the Upsert pages to bring in all the garages that this bus can be assigned to
    public class BusViewModel
    {
        public Bus Bus { get; set; }
        public IEnumerable<SelectListItem> GarageList { get; set; }
    }
}
