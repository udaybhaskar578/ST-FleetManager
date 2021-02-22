using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagementSystem.Models.ViewModels
{
    public class BusQuickDetailViewModel
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public double OdometerReading { get; set; }
        public int MaximumCapacity { get; set; }
        public bool AirConditioning { get; set; }
        public string CurrentStatus { get; set; }
    }
}
