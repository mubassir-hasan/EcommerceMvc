using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_MVC_Core.Data;
using Ecommerce_MVC_Core.Models.Admin;
using Ecommerce_MVC_Core.Repository;
using Ecommerce_MVC_Core.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_MVC_Core.Controllers.Admin
{
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(
            IUnitOfWork unitOfWork
        )
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index(string search="")
        {
            List<CategoryListViewModel> model=new List<CategoryListViewModel>();
            var dbData =await _unitOfWork.Repository<Category>().GetAllIncludeAsync(x=>x.Categoris,p=>p.Products);
                
            if (!String.IsNullOrEmpty(search))
            {
                dbData=(IList<Category>) dbData.Where(x => x.Name.ToLower().Contains(search.ToLower()));
                ViewBag.SearchString = search;

            }

            foreach (var c in dbData)
            {
                CategoryListViewModel categoryList = new CategoryListViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Order=c.Order,
                    Description = c.Description,
                    CategoryId = c.CategoryId,
                    CategoryParentName = c.Categoris?.Name,
                    TotalProduct = c.Products?.Count ?? 0
                };
                model.Add(categoryList);
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddEditCategory(int id)
        {
            CategoryViewModel model = new CategoryViewModel
            {
                Categories =  _unitOfWork.Repository<Category>().GetAll().Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }).OrderBy(x => x.Text).ToList()
            };
            var totalCategory =await _unitOfWork.Repository<Category>().CountAsync();
            model.Order = totalCategory + 1;
            if (id>0)
            {
                Category category= await _unitOfWork.Repository<Category>().GetByIdAsync(id);
                if (category!=null)
                {
                    model.Name = category.Name;
                    model.CategoryId = category.CategoryId;
                    model.Description = category.Description;
                    model.Order = category.Order;
                }

            }
            return PartialView("_AddEditCategory",model);
        }

        [HttpPost]
        public async Task<IActionResult> AddEditCategory(int id,CategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("_AddEditCategory",model);
            }

            if (id>0)
            {
                Category category = await _unitOfWork.Repository<Category>().GetByIdAsync(id);
                if (category!=null)
                {
                    category.Name = model.Name;
                    category.Description = model.Name;
                    category.CategoryId = model.CategoryId;
                    category.Order = model.Order;
                    category.ModifiedDate=DateTime.Now;
                    await _unitOfWork.Repository<Category>().UpdateAsync(category);
                }
            }
            else
            {
                Category category = new Category
                {
                    Name = model.Name,
                    Description = model.Description,
                    Order = model.Order,
                    CategoryId = model.CategoryId,
                    AddedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                };
                await _unitOfWork.Repository<Category>().InsertAsync(category);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Category category = await _unitOfWork.Repository<Category>().GetByIdAsync(id);

            return PartialView("_DeleteCategory", category?.Name);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, IFormCollection form)
        {
            var category = await _unitOfWork.Repository<Category>().GetByIdAsync(id);
            if (category != null)
            {
                await _unitOfWork.Repository<Category>().DeleteAsync(category);

            }
            return RedirectToAction("Index");
        }
    }
}