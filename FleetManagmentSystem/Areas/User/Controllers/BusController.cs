using FleetManagementSystem.DataAccess.Repository.IRepository;
using FleetManagementSystem.Models;
using FleetManagementSystem.Models.ViewModel;
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

        //Get the details of the bus for a given id
        [HttpGet]
        public IActionResult Details(int? id)
        {
            var bus = new Bus();
            try
            {
                if (id == null)
                {
                    return PartialView("_BusModalPartial", bus);
                }
                bus = _unitOfWork.Bus.Get(id.GetValueOrDefault());
                if (bus == null)
                {
                    throw new Exception("Unable to Find the bus");
                }
            }
            catch (Exception ex)
            {
                var evm = new ErrorViewModel();
                evm.ErrorMessage = ex.Message.ToString();
                return View("Error", evm);
            }

            return PartialView("_BusModalPartial", bus);
        }

        //Udpate the details of the bus in the database

        [HttpPost]
        public IActionResult Details(BusQuickDetailViewModel bus)
        {
            var objFromDb = _unitOfWork.Bus.Get(bus.Id);
            try
            {
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
            }
            catch (Exception ex)
            {
                var evm = new ErrorViewModel();
                evm.ErrorMessage = ex.Message.ToString();
                return View("Error", evm);
            }



            return PartialView("_BusModalPartial", objFromDb);
        }

        //Gets the resale value of the bus when provided with a bus id
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

        //Calculates the resale value of the bus up on the value changes on the fields.
        [HttpPost]
        public IActionResult CalculateResaleValue([FromBody] BusQuickDetailViewModel busvm)
        {
            var resaleValue = Constants.CalculateResaleValue(busvm.Year, busvm.MaximumCapacity,
                                busvm.OdometerReading, busvm.AirConditioning, busvm.CurrentStatus);
            if (resaleValue != null)
            {
                return Json(new { success = true, message = resaleValue.ToString() });
            }
            return Json(new { success = false, message = "Currently the bus is not available for resale. <br/> Please revisit." });
        }
    }
}
