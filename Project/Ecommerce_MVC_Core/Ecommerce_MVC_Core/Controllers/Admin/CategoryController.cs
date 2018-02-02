using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_MVC_Core.Data;
using Ecommerce_MVC_Core.Models.Admin;
using Ecommerce_MVC_Core.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce_MVC_Core.Controllers.Admin
{
    public class CategoryController : Controller
    {
        private readonly IRepository<Category> _repoCategory;

        public CategoryController(
            IRepository<Category> repoCategory
        )
        {
            _repoCategory = repoCategory;
        }

        public IActionResult Index(string search="")
        {
            List<CategoryListViewModel> model=new List<CategoryListViewModel>();
            if (!String.IsNullOrEmpty(search))
            {
                _repoCategory.GetAll().Where(x => x.Name.ToLower().Contains(search.ToLower())).ToList().ForEach(c =>
                {
                    CategoryListViewModel categoryList=new CategoryListViewModel
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description,
                        CategoryId = c.CategoryId,
                        CategoryParentName = c.CategoryId==null?"":
                        _repoCategory.GetAll().First(x => x.Id == c.CategoryId).Name,
                        TotalProduct = c.Products?.Count ?? 0
                    };
                    model.Add(categoryList);
                });
                ViewBag.SearchString = search;

            }
            else
            {
                _repoCategory.GetAll().ToList().ForEach(c =>
                {
                    CategoryListViewModel categoryList = new CategoryListViewModel
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description,
                        CategoryId = c.CategoryId,
                        CategoryParentName = c.CategoryId == null ? "" :
                            _repoCategory.GetAll().First(x => x.Id == c.CategoryId).Name,
                        TotalProduct = c.Products?.Count ?? 0
                    };
                    model.Add(categoryList);
                });

            }
            return View(model);
        }

        [HttpGet]
        public IActionResult AddEditCategory(int id)
        {
            CategoryViewModel model = new CategoryViewModel
            {
                Categories = _repoCategory.GetAll().Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }).OrderBy(x => x.Text).ToList()
            };
            if (id>0)
            {
                Category category=_repoCategory.GetById(id);
                if (category!=null)
                {
                    model.Name = category.Name;
                    model.CategoryId = category.CategoryId;
                    model.Description = category.Description;
                }

            }
            return PartialView("_AddEditCategory",model);
        }

        [HttpPost]
        public IActionResult AddEditCategory(int id,CategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("_AddEditCategory",model);
            }

            if (id>0)
            {
                Category category = _repoCategory.GetById(id);
                if (category!=null)
                {
                    category.Name = model.Name;
                    category.Description = model.Name;
                    category.CategoryId = model.CategoryId;
                    
                    category.ModifiedDate=DateTime.Now;
                    _repoCategory.Update(category);
                }
            }
            else
            {
                Category category=new Category
                {
                    Name = model.Name,
                    Description = model.Description,
                    CategoryId = model.CategoryId,
                    AddedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                };
                _repoCategory.Insert(category);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Category category = _repoCategory.GetById(id);

            return PartialView("_DeleteCategory", category?.Name);
        }

        [HttpPost]
        public IActionResult Delete(int id, IFormCollection form)
        {
            Category category = _repoCategory.GetById(id);
            if (category != null)
            {
                _repoCategory.Delete(category);

            }
            return RedirectToAction("Index");
        }
    }
}