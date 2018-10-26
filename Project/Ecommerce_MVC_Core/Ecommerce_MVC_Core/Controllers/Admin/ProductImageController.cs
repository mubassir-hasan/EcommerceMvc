using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_MVC_Core.Data;
using Ecommerce_MVC_Core.Models.Admin;
using Ecommerce_MVC_Core.Repository;
using Ecommerce_MVC_Core.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce_MVC_Core.Controllers.Admin
{
    public class ProductImageController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHostingEnvironment _hosingEnv;

        public ProductImageController(
            IHostingEnvironment hostingEnv,
            IUnitOfWork unitOfWork
        )
        {
            _hosingEnv = hostingEnv;
            _unitOfWork = unitOfWork;
        }

        

        public IActionResult Index(string search="")
        {
            IEnumerable<ProductImageListViewModel> model = new List<ProductImageListViewModel>();
            if (String.IsNullOrEmpty(search))
            {
                model = GetAllProductImageList();
            }
            else
            {
                model = GetAllProductImageList().Where(x => x.ProductName.ToLower().Contains(search.ToLower()) || x.Title.ToLower().Contains(search.ToLower()));
                ViewBag.SearchString = search;
            }

            return View(model);
        }


        public List<ProductImageListViewModel> GetAllProductImageList()
        {
            List<ProductImageListViewModel> productImageList = new List<ProductImageListViewModel>();
            _unitOfWork.Repository<ProductImage>().GetAllInclude(p=>p.Product).OrderByDescending(x=>x.AddedDate).ToList().ForEach(p =>
            {
                ProductImageListViewModel productImage=new ProductImageListViewModel
                {
                    Id = p.Id,
                    ProductId = p.Id,
                    Title = p.Title,
                    ImagePath = p.ImagePath,
                    ProductName = p.Product.Name
                };
                productImageList.Add(productImage);
            });
            return productImageList;
        }

        [HttpGet]
        public async Task<IActionResult> AddEditProductImage(int id)
        {
            ProductImageViewModel model = new ProductImageViewModel();
            model.Categories = GetAllCategories();
            if (id > 0)
            {
                ProductImage pImage = await _unitOfWork.Repository<ProductImage>().GetByIdAsync(id);
                model.ProductId = pImage.ProductId;
                model.Title = pImage.Title;
                
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddEditProductImage(int id, ProductImageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Something wrong!!!");
                return View(model);
            }

            if (id > 0)
            {
                ProductImage pImage = await _unitOfWork.Repository<ProductImage>().GetByIdAsync(id);
                if (pImage != null)
                {
                    pImage.ProductId = model.ProductId;
                    pImage.Title = model.Title;
                    pImage.ModifiedDate = DateTime.Now;

                    if (model.ImageFile != null && (model.ImageFile.ContentType == "image/png" || model.ImageFile.ContentType == "image/jpg" || model.ImageFile.ContentType == "image/jpeg"))
                    {

                        var uploads = Path.Combine(_hosingEnv.WebRootPath, "uploads/ProductImages");
                        var fileName = Path.Combine(uploads, _unitOfWork.Repository<Product>().Find(x => x.Id == model.ProductId).Name + "_" + Guid.NewGuid().ToString().Substring(0, Guid.NewGuid().ToString().IndexOf("-", StringComparison.Ordinal)) + ".png");
                        pImage.ImagePath = Path.GetFileName(fileName);
                        
                        using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                        {
                            await model.ImageFile.CopyToAsync(fileStream);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Wrong file Type. Please Upload only Image file");
                    }
                    await _unitOfWork.Repository<ProductImage>().UpdateAsync(pImage);
                }
                

            }
            else
            {
                ProductImage pImage =new ProductImage
                {
                    ProductId = model.ProductId,
                    Title = model.Title,
                    AddedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                };
                if (model.ImageFile != null &&(model.ImageFile.ContentType== "image/png"|| model.ImageFile.ContentType == "image/jpg" || model.ImageFile.ContentType == "image/jpeg") )
                {

                    var uploads = Path.Combine(_hosingEnv.WebRootPath, "uploads/ProductImages");
                    var fileName = Path.Combine(uploads, _unitOfWork.Repository<Product>().Find(x => x.Id == model.ProductId).Name + "_" + Guid.NewGuid().ToString().Substring(0, Guid.NewGuid().ToString().IndexOf("-", StringComparison.Ordinal)) + ".png");
                    pImage.ImagePath = Path.GetFileName(fileName);
                    await _unitOfWork.Repository<ProductImage>().InsertAsync(pImage);
                    using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(fileStream);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Wrong file Type. Please Upload only Image file");
                }
            }

            return RedirectToAction(nameof(AddEditProductImage));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            ProductImage productImage = await _unitOfWork.Repository<ProductImage>().GetByIdAsync(id);

            return PartialView("_DeleteProductImage", productImage?.Title);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, IFormCollection form)
        {
            ProductImage productImage = await _unitOfWork.Repository<ProductImage>().GetByIdAsync(id);
            if (productImage != null)
            {
                await _unitOfWork.Repository<ProductImage>().DeleteAsync(productImage);
                var fullPath = Path.Combine(_hosingEnv.WebRootPath, "uploads/ProductImages/"+productImage.ImagePath);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

            }
            return RedirectToAction("Index");
        }


        public List<SelectListItem> GetAllCategories()
        {
            var category =  _unitOfWork.Repository<Category>().GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();
            category.Add(new SelectListItem{Text = "--Select--",Value = "0",Selected = true});
            return category;
        }

        
        //AutoComplete for Product
        [HttpGet]
        public JsonResult GetProducts(string term)
        {
            term = term.ToLower();
            var productList = _unitOfWork.Repository<Product>().GetAll().Where(x => x.Name.ToLower().StartsWith(term)).Select(x => new { label = x.Name, val = x.Id }).ToList();


            return Json(productList);
        }

        //Showing image
        public IActionResult ImageView(int id)
        {
            ProductImage img = _unitOfWork.Repository<ProductImage>().GetById(id);
            string fileName = img.ImagePath;
            return PartialView("_ImageView",fileName);
        }

    }
}