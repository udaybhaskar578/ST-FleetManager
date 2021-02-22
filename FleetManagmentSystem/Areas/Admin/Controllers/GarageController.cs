using FleetManagementSystem.DataAccess.Repository.IRepository;
using FleetManagementSystem.Models;
using FleetManagementSystem.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FleetManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GarageController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public GarageController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            GarageViewModel garagevm = new GarageViewModel()
            {
                Garage = new Garage(),
                ClosestGarageList = _unitOfWork.Garage.GetAll(u=>u.Id != id.GetValueOrDefault()).Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };
            if (id == null)
            {
                return View(garagevm);
            }
            garagevm.Garage = _unitOfWork.Garage.Get(id.GetValueOrDefault());
            if (garagevm.Garage == null)
            {
                return NotFound();
            }
            return View(garagevm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(GarageViewModel garagevm)
        {
            if (ModelState.IsValid)
            {
                if (garagevm.Garage.Id == 0)
                {
                    _unitOfWork.Garage.Add(garagevm.Garage);

                }
                else
                {

                    _unitOfWork.Garage.Update(garagevm.Garage);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(garagevm);
        }

        #region
        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Garage.GetAll();
            return Json(new { data = allObj });
        }

        #endregion
    }
}
