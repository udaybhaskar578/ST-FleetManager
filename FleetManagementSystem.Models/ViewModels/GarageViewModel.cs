using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagementSystem.Models.ViewModels
{
    public class GarageViewModel
    {
        public Garage Garage { get; set; }
        public IEnumerable<SelectListItem> ClosestGarageList { get; set; }

    }
}
