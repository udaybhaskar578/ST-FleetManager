using FleetManagementSystem.DataAccess.Repository.IRepository;
using FleetManagementSystem.Models;
using FleetManagementSystem.Models.ViewModels;
using FleetManagementSystem.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FleetManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Constants.Role_Admin + "," + Constants.Role_Employee)]
    public class BusController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public BusController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Bus bus = new Bus();
            if (id == null)
            {
                bus.CurrentStatus = Constants.BusStatus["Ready for use"];
                return View(bus);
            }
            bus = _unitOfWork.Bus.Get(id.GetValueOrDefault());
            if (bus == null)
            {
                return NotFound();
            }
            return View(bus);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Bus bus)
        {
            if (ModelState.IsValid)
            {

                string webRootPath = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                bus.ResaleValue = Constants.CalculateResaleValue(bus.Year, bus.MaximumCapacity,
                                    bus.OdometerReading, bus.AirConditioning,
                                    bus.CurrentStatus).GetValueOrDefault();
                bus.ResaleValue = Math.Round(bus.ResaleValue, 2);
                if (files.Count > 0)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"images\buses");
                    var extension = Path.GetExtension(files[0].FileName);
                    if (bus.ImageUrl != null)
                    {
                        //Removing previous image
                        var imagePath = Path.Combine(webRootPath, bus.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }

                    }
                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStreams);
                    }
                    bus.ImageUrl = @"\images\buses\" + fileName + extension;

                }
                else
                {
                    if (bus.Id != 0)
                    {
                        
                        Bus objFromDb = _unitOfWork.Bus.Get(bus.Id);
                        bus.ImageUrl = objFromDb.ImageUrl;
                    }
                }
                if (bus.Id == 0)
                {
                    bus.CurrentStatus = Constants.BusStatus["Ready for use"];
                    _unitOfWork.Bus.Add(bus);

                }
                else
                {
                    _unitOfWork.Bus.Update(bus);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                if (bus.Id != 0)
                {
                    bus = _unitOfWork.Bus.Get(bus.Id);
                }
            }
            return View(bus);
        }

        public IActionResult Assign(int? id)
        {

            GarageAssignmentViewModel gavm = new GarageAssignmentViewModel()
            {
                GarageAssignment = new GarageAssignment(),
                
                GarageList = _unitOfWork.Garage.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                BusList = _unitOfWork.Bus.GetAll().Select(i => new SelectListItem
                {
                    Text = i.RegistrationNumber,
                    Value = i.Id.ToString()
                })
            };
            if (id == null)
            {
                return View(gavm);
            }
            
            gavm.GarageAssignment.BusId = id.GetValueOrDefault();
            gavm.GarageAssignment.Bus = _unitOfWork.Bus.Get(id.GetValueOrDefault());

            //Check if the bus is parked in any garage
            var assignedGarage = _unitOfWork.GarageAssignment.GetFirstOrDefault(u => u.BusId == id && u.CheckOut == null, includeProperties: "Garage,Bus");
            if(assignedGarage != null)
            {
                gavm.GarageAssignment.GarageId = assignedGarage.GarageId;
                gavm.GarageAssignment.Garage = assignedGarage.Garage;
                gavm.GarageAssignment.CheckIn = assignedGarage.CheckIn;
            }
            if (gavm.GarageAssignment.Bus == null)
            {
                return NotFound();
            }
            return View(gavm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Assign(GarageAssignmentViewModel gavm)
        {
            if (ModelState.IsValid)
            {
                if (gavm.GarageAssignment.Id == 0)
                {
                    _unitOfWork.GarageAssignment.Add(gavm.GarageAssignment);
                }
                else
                {
                    _unitOfWork.GarageAssignment.Update(gavm.GarageAssignment);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
                
            }
            return View(gavm);
        }


        #region
        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Bus.GetAll();
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Bus.Get(id);
            //Need To Remove the Garage Entry as well
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            //Removing previous image
            string webRootPath = _hostEnvironment.WebRootPath;
            var imagePath = Path.Combine(webRootPath, objFromDb.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
            //Need To Remove the Garage Entry as well
            _unitOfWork.Bus.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }
}
