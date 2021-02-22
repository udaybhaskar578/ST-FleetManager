using FleetManagementSystem.DataAccess.Repository.IRepository;
using FleetManagementSystem.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FleetManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Constants.Role_Admin + "," + Constants.Role_Employee)]
    public class GarageAssignmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public GarageAssignmentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region
        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.GarageAssignment.GetAll(u=>u.CheckOut == null, includeProperties:"Bus,Garage");
            return Json(new { data = allObj });
        }
        [HttpPost]
        public IActionResult Checkout([FromBody] string id)
        {
            var objFromDb = _unitOfWork.GarageAssignment.GetFirstOrDefault(u => u.Id == Int32.Parse(id));
            var requests = _unitOfWork.MaintenanceLineItem.GetAll(u => u.BusId == objFromDb.BusId && u.GarageId == objFromDb.GarageId && u.CompletedOn == null);
            if(requests.Count() > 0)
            {
                return Json(new { success = false, message = "Please Check with the service staff as there the bus is undergoing maintenance" });
            }
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while changing the status" });
            }
            objFromDb.CheckOut = DateTime.Today;
            _unitOfWork.GarageAssignment.Update(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Checked out the vehicle successfully" });
        }
        #endregion
    }
}
