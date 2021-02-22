using FleetManagementSystem.DataAccess.Repository.IRepository;
using FleetManagementSystem.Models;
using FleetManagementSystem.Models.ViewModels;
using FleetManagementSystem.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FleetManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Constants.Role_Admin + "," + Constants.Role_Employee + "," + Constants.Role_Technician)]
    public class MaintenanceLineItemController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public MaintenanceLineItemController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            MaintenanceLineItemViewModel mliVM = new MaintenanceLineItemViewModel()
            {
                MaintenanceLineItem = new MaintenanceLineItem(),
                BusList = _unitOfWork.GarageAssignment.GetAll(b => b.CheckOut == null, includeProperties: "Bus").
                          Select(i => new SelectListItem
                          {
                              Text = i.Bus.RegistrationNumber,
                              Value = i.Bus.Id.ToString()
                          }),
                RequestTypeList = _unitOfWork.MaintenanceRequestType.GetAll(b=>b.IsActive == true).Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };

            if (id == null)
            {
                return View(mliVM);
            }
            mliVM.MaintenanceLineItem = _unitOfWork.MaintenanceLineItem.Get(id.GetValueOrDefault());
            if (mliVM == null)
            {
                return NotFound();
            }
            return View(mliVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(MaintenanceLineItemViewModel mliVM)
        {
            if (ModelState.IsValid)
            {
                if (mliVM.MaintenanceLineItem.Id == 0)
                {
                    var garageAssignment = _unitOfWork.GarageAssignment.
                                GetFirstOrDefault(u => u.BusId == mliVM.MaintenanceLineItem.BusId && u.CheckOut == null, includeProperties: "Bus");

                    mliVM.MaintenanceLineItem.GarageId = garageAssignment.GarageId;
                    mliVM.MaintenanceLineItem.Bus = garageAssignment.Bus;
                    mliVM.MaintenanceLineItem.Status = Constants.RequestStatus["Waiting for Technician"];
                    mliVM.MaintenanceLineItem.Bus.CurrentStatus = Constants.BusStatus["Scheduled for maintenance"];
                    _unitOfWork.MaintenanceLineItem.Add(mliVM.MaintenanceLineItem);
                }
                else
                {
                    var mli = _unitOfWork.MaintenanceLineItem.GetFirstOrDefault(u => u.Id == mliVM.MaintenanceLineItem.Id, includeProperties: "Bus");
                    mli.Status = mliVM.MaintenanceLineItem.Status;
                    if (mli.Status == Constants.RequestStatus["Complete"])
                    {
                        mli.CompletedOn = DateTime.Today.Date;
                        var pendingMLI = _unitOfWork.MaintenanceLineItem.GetAll(u => u.Id != mli.Id
                                        && u.BusId == mli.BusId && u.GarageId == mli.GarageId
                                        && u.Status != Constants.RequestStatus["Complete"]);
                        if (pendingMLI.Count() == 0)
                        {
                            mli.Bus.CurrentStatus = Constants.BusStatus["Ready for use"];
                        }

                    }
                    if (mli.Status == Constants.RequestStatus["In Progress"])
                    {
                        mli.Bus.CurrentStatus = Constants.BusStatus["Undergoing repairs"];

                    }
                    _unitOfWork.MaintenanceLineItem.Update(mli);
                }
                _unitOfWork.Save();
                _unitOfWork.Dispose();
                return RedirectToAction(nameof(Index));

            }
            return View(mliVM);
        }
        #region
        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.MaintenanceLineItem.GetAll(u => u.Status != Constants.RequestStatus["Complete"],
                            includeProperties: "Bus,Garage,MaintenanceRequestType");
            return Json(new { data = allObj });
        }

        [HttpPost]
        public IActionResult Complete([FromBody] string id)
        {
            var objFromDb = _unitOfWork.MaintenanceLineItem.GetFirstOrDefault(u => u.Id == Int32.Parse(id),includeProperties:"Bus");
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while changing the status" });
            }
            objFromDb.CompletedOn = DateTime.Today.Date;
            objFromDb.Status = Constants.RequestStatus["Complete"];
            var pendingMLI = _unitOfWork.MaintenanceLineItem.GetAll(u => u.Id != objFromDb.Id
                && u.BusId == objFromDb.BusId && u.GarageId == objFromDb.GarageId
                && u.Status != Constants.RequestStatus["Complete"]);
            if (pendingMLI.Count() == 0)
            {
                objFromDb.Bus.CurrentStatus = Constants.BusStatus["Ready for use"];
            }
            _unitOfWork.MaintenanceLineItem.Update(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Maintenance Line Item marked as completed" });

        }


        #endregion
    }
}
