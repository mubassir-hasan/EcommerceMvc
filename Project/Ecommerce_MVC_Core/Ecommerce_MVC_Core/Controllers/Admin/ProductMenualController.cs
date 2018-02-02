using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_MVC_Core.Data;
using Ecommerce_MVC_Core.Models.Admin;
using Ecommerce_MVC_Core.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_MVC_Core.Controllers.Admin
{
    public class ProductMenualController : Controller
    {
        private readonly IRepository<ProductManual> _repoProductMenual;
        private readonly IRepository<Product> _repoProduct;
        private readonly IRepository<Category> _repoCategory;
        private readonly IHostingEnvironment _hostingEnv;

        public ProductMenualController(IRepository<ProductManual> repoProductMenual, IRepository<Product> repoProduct, IRepository<Category> repoCategory, IHostingEnvironment hostingEnv)
        {
            _repoProductMenual = repoProductMenual;
            _repoProduct = repoProduct;
            _repoCategory = repoCategory;
            _hostingEnv = hostingEnv;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddEditProductManual(int id)
        {
            ProductMenualViewModel model=new ProductMenualViewModel();
            if (id>0)
            {
                ProductManual productManual = _repoProductMenual.GetById(id);
                model.ProductId = productManual.ProductId;
                model.ProductName = _repoProduct.GetAll().First(x => x.Id == productManual.ProductId).Name;

            }
            return View(model);
        }

        public async  Task<IActionResult> AddEditProductManual(int id, ProductMenualViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("","Something Wrong ");
                return View(model);
            }
            if (id>0)
            {
                ProductManual pManual = _repoProductMenual.GetById(id);
                pManual.ProductId = model.ProductId;
                pManual.ModifiedDate=DateTime.Now;
            }
            else
            {
                ProductManual productManual=new ProductManual
                {
                    AddedDate = DateTime.Now,
                    ProductId = model.ProductId,
                    ModifiedDate = DateTime.Now
                };

                if (model.ProductFile != null && model.ProductFile.ContentType== "application/pdf")
                {
                    
                    var uploads = Path.Combine(_hostingEnv.WebRootPath, "uploads/ProductManuals");
                    var fileName = Path.Combine(uploads, model.ProductName + "_" + model.ProductId + ".pdf");
                    productManual.ManualName = Path.GetFileName(fileName);
                    _repoProductMenual.Insert(productManual);
                    using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                    {
                        await model.ProductFile.CopyToAsync(fileStream);
                    }
                    
                    //await model.ImageFile.CopyToAsync(new FileStream(fileName,FileMode.Create));
                }
                else
                {
                    ModelState.AddModelError("","Wrong file Type. Please Upload only PDF file");
                }
                
                    
                
            }

            return View();
        }

        //AutoComplete for Product
        [HttpGet]
        public JsonResult GetProducts(string term)
        {
            term = term.ToLower();
            var productList = _repoProduct.GetAll().Where(x => x.Name.ToLower().StartsWith(term)).Select(x => new { label = x.Name, val = x.Id }).ToList();


            return Json(productList);
        }
    }
}