using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_MVC_Core.Data;
using Ecommerce_MVC_Core.Models.Admin;
using Ecommerce_MVC_Core.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_MVC_Core.Controllers.Admin
{
    
    public class BrandController : Controller
    {
        private readonly IRepository<Brand> _repoBrand;
        private readonly IRepository<Product> _repoProduct;

        public BrandController(
            IRepository<Brand> repoBrand,
            IRepository<Product> repoProduct
        )
        {
            _repoBrand = repoBrand;
            _repoProduct = repoProduct;
        }

        public IActionResult Index(string search="")
        {
            List<BrandListViewModel> model = new List<BrandListViewModel>();
            if (!String.IsNullOrEmpty(search))
            {
                _repoBrand.GetAll().Where(x => x.Name.ToLower().Contains(search.ToLower())).ToList().ForEach(b =>
                {
                    BrandListViewModel brand = new BrandListViewModel
                    {
                        Id = b.Id,
                        Name = b.Name,
                        Description = b.Description,
                        TotalProduct = _repoProduct.GetAll().Count(x=>x.BrandId==b.Id)
                    };

                    model.Add(brand);
                });
                ViewBag.SearchString = search;
            }
            else
            {
                _repoBrand.GetAll().ToList().ForEach(b =>
                {
                    BrandListViewModel brand = new BrandListViewModel
                    {
                        Id = b.Id,
                        Name = b.Name,
                        Description = b.Description,
                        TotalProduct = _repoProduct.GetAll().Count(x => x.BrandId == b.Id)
                    };

                    model.Add(brand);
                });
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult AddEditBrand(int id)
        {
            BrandViewModel model=new BrandViewModel();
            if (id>0)
            {
                Brand brand = _repoBrand.GetById(id);
                model.Name = brand.Name;
                model.Description = brand.Description;
            }
            return PartialView("_AddEditBrand",model);
        }

        [HttpPost]
        public IActionResult AddEditBrand(int id, BrandViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("_AddEditBrand",model);
            }

            if (id>0)
            {
                Brand brand = _repoBrand.GetById(id);
                if (brand!=null)
                {
                    brand.Name = model.Name;
                    brand.Description = model.Description;
                    brand.ModifiedDate = DateTime.Now;
                }
                _repoBrand.Update(brand);
                
            }
            else
            {
                Brand brand=new Brand
                {
                    Name = model.Name,
                    Description = model.Description,
                    AddedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                };
                _repoBrand.Insert(brand);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Brand brand = _repoBrand.GetById(id);

            return PartialView("_DeleteBrand",brand?.Name);
        }

        [HttpPost]
        public IActionResult Delete(int id, IFormCollection form)
        {
            Brand brand = _repoBrand.GetById(id);
            if (brand!=null)
            {
                _repoBrand.Delete(brand);

            }
            return RedirectToAction("Index");
        }
    }
}