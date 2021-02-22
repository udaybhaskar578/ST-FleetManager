using FleetManagementSystem.DataAccess.Repository.IRepository;
using FleetManagementSystem.Models;
using FleetManagementSystem.Models.ViewModel;
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

        //Insert or Update action for Bus
        public IActionResult Upsert(int? id)
        {
            Bus bus = new Bus();
            try
            {
                //Check if it is an insert operation
                if (id == null)
                {
                    bus.CurrentStatus = Constants.BusStatus["Ready for use"];
                    return View(bus);
                }
                //Get the bus details from DB for update
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


            return View(bus);
        }

        //Insert/Update Bus in Database 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Bus bus)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Get Images
                    string webRootPath = _hostEnvironment.WebRootPath;
                    var files = HttpContext.Request.Form.Files;
                    //Calculate resale value of the bus
                    bus.ResaleValue = Constants.CalculateResaleValue(bus.Year, bus.MaximumCapacity,
                                        bus.OdometerReading, bus.AirConditioning,
                                        bus.CurrentStatus).GetValueOrDefault();
                    bus.ResaleValue = Math.Round(bus.ResaleValue, 2);
                    //If there is an image make necessary modifications
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
                        //Set Image URL if there is no update in the image
                        if (bus.Id != 0)
                        {

                            Bus objFromDb = _unitOfWork.Bus.Get(bus.Id);
                            bus.ImageUrl = objFromDb.ImageUrl;
                        }
                    }

                    //Insert New Bus
                    if (bus.Id == 0)
                    {
                        bus.CurrentStatus = Constants.BusStatus["Ready for use"];
                        _unitOfWork.Bus.Add(bus);

                    }
                    //Update Bus
                    else
                    {
                        _unitOfWork.Bus.Update(bus);
                    }
                    _unitOfWork.Save();
                    return RedirectToAction(nameof(Index));
                }
                //If Model is invalid return the bus entity back
                else
                {
                    if (bus.Id != 0)
                    {
                        bus = _unitOfWork.Bus.Get(bus.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                var evm = new ErrorViewModel
                {
                    ErrorMessage = ex.Message.ToString()
                };
                return View("Error", evm);
            }
            return View(bus);
        }

        //Assign bus to a garage
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
            try
            {
                if (id == null)
                {
                    return View(gavm);
                }

                gavm.GarageAssignment.BusId = id.GetValueOrDefault();
                gavm.GarageAssignment.Bus = _unitOfWork.Bus.Get(id.GetValueOrDefault());

                //Check if the bus is parked in any garage
                var assignedGarage = _unitOfWork.GarageAssignment.GetFirstOrDefault(u => u.BusId == id && u.CheckOut == null, includeProperties: "Garage,Bus");
                if (assignedGarage != null)
                {
                    gavm.GarageAssignment.GarageId = assignedGarage.GarageId;
                    gavm.GarageAssignment.Garage = assignedGarage.Garage;
                    gavm.GarageAssignment.CheckIn = assignedGarage.CheckIn;
                }
                if (gavm.GarageAssignment.Bus == null)
                {
                    throw new Exception("Unable to find the garage assignment for the bus");
                }
            }
            catch (Exception ex)
            {
                var evm = new ErrorViewModel
                {
                    ErrorMessage = ex.Message.ToString()
                };
                return View("Error", evm);
            }
            return View(gavm);
        }

        //Update Database value with the garage assignment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Assign(GarageAssignmentViewModel gavm)
        {
            try
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
            }
            catch(Exception ex)
            {
                var evm = new ErrorViewModel
                {
                    ErrorMessage = ex.Message.ToString()
                };
                return View("Error", evm);
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
