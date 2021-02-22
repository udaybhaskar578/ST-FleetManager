using FleetManagementSystem.DataAccess.Repository.IRepository;
using FleetManagementSystem.Models;
using FleetManagementSystem.Models.ViewModel;
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

        //Insert or Update action for Garage
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
            try
            {
                if (id == null)
                {
                    return View(garagevm);
                }
                garagevm.Garage = _unitOfWork.Garage.Get(id.GetValueOrDefault());
                if (garagevm.Garage == null)
                {
                    throw new Exception("Unable to find the garage");
                }
            }
            catch(Exception ex)
            {
                var evm = new ErrorViewModel();
                evm.ErrorMessage = ex.Message.ToString();
                return View("Error", evm);
            }

            return View(garagevm);
        }


        //Insert/Update Garage in Database 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(GarageViewModel garagevm)
        {
            try
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
            }catch(Exception ex)
            {
                var evm = new ErrorViewModel();
                evm.ErrorMessage = ex.Message.ToString();
                return View("Error", evm);
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
