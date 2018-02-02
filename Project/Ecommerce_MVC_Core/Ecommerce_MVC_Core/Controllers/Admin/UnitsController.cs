using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_MVC_Core.Data;
using Ecommerce_MVC_Core.Models.Admin;
using Ecommerce_MVC_Core.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_MVC_Core.Controllers.Admin
{
    public class UnitsController : Controller
    {
       

        private IRepository<Unit> _repoUnit { get; set; }
        private IRepository<Product> _repoProduct { get; set; }
        
        public UnitsController(IRepository<Unit> repoUnit, IRepository<Product> repoProduct)
        {
            _repoProduct = repoProduct;
            _repoUnit = repoUnit;
        }

        public IActionResult Index(string search="")
        {
            List<UnitListViewModel> model = new List<UnitListViewModel>();

            if (!String.IsNullOrEmpty(search))
            {
                _repoUnit.GetAll().Where(x => x.Name.ToLower().Contains(search.ToLower())).ToList().ForEach(b =>
                {
                    UnitListViewModel unit = new UnitListViewModel
                    {
                        Id = b.Id,
                        Name = b.Name,
                        Description = b.Description,
                        TotalProducts = _repoProduct.GetAll().Count(x => x.UnitId == b.Id)
                    };

                    ViewBag.SearchString = search;
                    model.Add(unit);
                });

            }
            else
            {
                _repoUnit.GetAll().ToList().ForEach(b =>
                {
                    UnitListViewModel unit = new UnitListViewModel
                    {
                        Id = b.Id,
                        Name = b.Name,
                        Description = b.Description,
                        TotalProducts = _repoProduct.GetAll().Count(x => x.UnitId == b.Id)
                    };

                    model.Add(unit);
                });
            }


            return View(model);
        }

        [HttpGet]
        public IActionResult AddEditUnit(int id)
        {
            UnitViewModel model = new UnitViewModel();
            if (id > 0)
            {
                Unit payment = _repoUnit.GetById(id);
                model.Name = payment.Name;
                model.Description = payment.Description;
            }

            return PartialView("_AddEditUnit", model);
        }


        [HttpPost]
        public IActionResult AddEditUnit(int id, UnitViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("_AddEditUnit", model);
            }

            if (id > 0)
            {
                Unit unit = _repoUnit.GetById(id);
                if (unit != null)
                {
                    unit.Name = model.Name;
                    unit.ModifiedDate = DateTime.Now;
                    unit.Description = model.Description;
                    _repoUnit.Update(unit);
                }
            }
            else
            {
                Unit unit = new Unit
                {
                    Name = model.Name,
                    ModifiedDate = DateTime.Now,
                    AddedDate = DateTime.Now,
                    Description = model.Description,
                };
                _repoUnit.Insert(unit);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult DeleteUnit(int id)
        {
            Unit unit = _repoUnit.GetById(id);

            return PartialView("_DeleteUnit", unit?.Name);
        }

        [HttpPost]
        public IActionResult DeleteUnit(int id, IFormCollection form)
        {
            Unit unit = _repoUnit.GetById(id);
            if (unit != null)
            {
                _repoUnit.Delete(unit);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}