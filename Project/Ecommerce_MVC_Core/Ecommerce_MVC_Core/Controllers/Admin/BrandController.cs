using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_MVC_Core.Data;
using Ecommerce_MVC_Core.Models.Admin;
using Ecommerce_MVC_Core.Repository;
using Ecommerce_MVC_Core.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_MVC_Core.Controllers.Admin
{
    
    public class BrandController : Controller
    {
        

        private readonly IUnitOfWork _unitOfWork;

        public BrandController(
            IUnitOfWork unitOfWork
        )
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index(string search="")
        {
            List<BrandListViewModel> model = new List<BrandListViewModel>();
            var dbBrand = await _unitOfWork.Repository<Brand>().GetAllIncludeAsync(x => x.Products);

            model=dbBrand.Select(b => new BrandListViewModel
                {
                    Id = b.Id,
                    Name = b.Name,
                    Description = b.Description,
                    TotalProduct = b.Products.Count
                }).ToList();

            if (!String.IsNullOrEmpty(search)) { 
                model=model.Where(x => x.Name.ToLower().Contains(search.ToLower())).ToList();
                ViewBag.SearchString = search;
            return View(model);
            }
            
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddEditBrand(int id)
        {
            BrandViewModel model=new BrandViewModel();
            if (id>0)
            {
                Brand brand =await _unitOfWork.Repository<Brand>().GetByIdAsync(id);
                model.Name = brand.Name;
                model.Description = brand.Description;
            }
            return PartialView("_AddEditBrand",model);
        }

        [HttpPost]
        public async Task<IActionResult> AddEditBrand(int id, BrandViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("_AddEditBrand",model);
            }

            if (id>0)
            {
                Brand brand = await _unitOfWork.Repository<Brand>().GetByIdAsync(id);
                if (brand!=null)
                {
                    brand.Name = model.Name;
                    brand.Description = model.Description;
                    brand.ModifiedDate = DateTime.Now;
                }
                 _unitOfWork.Repository<Brand>().Update(brand);
                
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
                await _unitOfWork.Repository<Brand>().InsertAsync(brand);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Brand brand = await _unitOfWork.Repository<Brand>().GetByIdAsync(id);

            return PartialView("_DeleteBrand",brand?.Name);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, IFormCollection form)
        {
            Brand brand = await _unitOfWork.Repository<Brand>().GetByIdAsync(id);
            if (brand!=null)
            {
                await _unitOfWork.Repository<Brand>().DeleteAsync(brand);

            }
            return RedirectToAction("Index");
        }
    }
}