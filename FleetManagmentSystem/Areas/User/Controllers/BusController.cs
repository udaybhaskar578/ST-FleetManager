using FleetManagementSystem.DataAccess.Repository.IRepository;
using FleetManagementSystem.Models;
using FleetManagementSystem.Models.ViewModels;
using FleetManagementSystem.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FleetManagementSystem.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = Constants.Role_Admin + "," + Constants.Role_Employee + "," + Constants.Role_Technician)]
    public class BusController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public BusController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            
            IEnumerable<Bus> busList = _unitOfWork.Bus.GetAll();
            return View(busList);
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            var bus = new Bus();
            if (id == null)
            {
                return PartialView("_BusModalPartial", bus);
            }
            bus = _unitOfWork.Bus.Get(id.GetValueOrDefault());
            if (bus == null)
            {
                return NotFound();
            }
            return PartialView("_BusModalPartial", bus);
        }

        [HttpPost]
        public IActionResult Details(BusQuickDetailViewModel bus)
        {
            var objFromDb = _unitOfWork.Bus.Get(bus.Id);
            if (objFromDb != null)
            {
                objFromDb.Year = bus.Year;
                objFromDb.OdometerReading = bus.OdometerReading;
                objFromDb.MaximumCapacity = bus.MaximumCapacity;
                objFromDb.ResaleValue = Math.Round(Constants.CalculateResaleValue(objFromDb.Year, objFromDb.MaximumCapacity,
                        objFromDb.OdometerReading, objFromDb.AirConditioning,
                        objFromDb.CurrentStatus).GetValueOrDefault(), 2);
                _unitOfWork.Bus.Update(objFromDb);
                _unitOfWork.Save();
            }

            return PartialView("_BusModalPartial", objFromDb);
        }

        [HttpGet]
        public IActionResult GetResaleValue(int? id)
        {
            var bus = _unitOfWork.Bus.Get(id.GetValueOrDefault());
            var resaleValue = Constants.CalculateResaleValue(bus.Year, bus.MaximumCapacity,
                                bus.OdometerReading, bus.AirConditioning, bus.CurrentStatus);
            if (resaleValue != null)
            {
                return Json(new { success = true, message = resaleValue });
            }
            return Json(new { success = false, message = "Currently the bus is not available for resale. <br/> Please revisit." });
        }


        [HttpPost]
        public IActionResult CalculateResaleValue([FromBody] BusQuickDetailViewModel busvm)
        {
            var resaleValue = Constants.CalculateResaleValue(busvm.Year, busvm.MaximumCapacity,
                                busvm.OdometerReading, busvm.AirConditioning, busvm.CurrentStatus);
            if (resaleValue != null)
            {
                return Json(new { success = true, message = resaleValue.ToString()});
            }
            return Json(new { success = false, message = "Currently the bus is not available for resale. <br/> Please revisit." });
        }
    }
}
