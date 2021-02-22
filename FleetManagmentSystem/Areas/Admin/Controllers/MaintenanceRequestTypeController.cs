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
    [Authorize(Roles = Constants.Role_Admin)]
    public class MaintenanceRequestTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public MaintenanceRequestTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            MaintenanceRequestType maintenanceRequestType = new MaintenanceRequestType();
            if (id == null)
            {
                return View(maintenanceRequestType);
            }
            maintenanceRequestType = _unitOfWork.MaintenanceRequestType.Get(id.GetValueOrDefault());
            if (maintenanceRequestType == null)
            {
                return NotFound();
            }
            return View(maintenanceRequestType);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(MaintenanceRequestType MaintenanceRequestType)
        {
            if (ModelState.IsValid)
            {
                if (MaintenanceRequestType.Id == 0)
                {
                    _unitOfWork.MaintenanceRequestType.Add(MaintenanceRequestType);

                }
                else
                {

                    _unitOfWork.MaintenanceRequestType.Update(MaintenanceRequestType);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(MaintenanceRequestType);
        }

        #region
        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.MaintenanceRequestType.GetAll();
            return Json(new { data = allObj });
        }

        [HttpPost]
        public IActionResult ToggleActiveStatus([FromBody] string id)
        {
            var objFromDb = _unitOfWork.MaintenanceRequestType.GetFirstOrDefault(u => u.Id == Int32.Parse(id));
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while changing the status" });
            }
            objFromDb.IsActive = !objFromDb.IsActive;
            _unitOfWork.MaintenanceRequestType.Update(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Maintenance Type Status changed successful" });

        }

        #endregion
    }
}
