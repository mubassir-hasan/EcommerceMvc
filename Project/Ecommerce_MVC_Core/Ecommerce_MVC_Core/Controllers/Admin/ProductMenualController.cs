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
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_MVC_Core.Controllers.Admin
{
    public class ProductMenualController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHostingEnvironment _hostingEnv;

        public ProductMenualController(IUnitOfWork unitOfWork, IHostingEnvironment hostingEnv)
        {
            _unitOfWork = unitOfWork;
            _hostingEnv = hostingEnv;

        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async  Task<IActionResult> AddEditProductManual(int id)
        {
            ProductMenualViewModel model=new ProductMenualViewModel();
            if (id>0)
            {
                ProductManual productManual =await _unitOfWork.Repository<ProductManual>().GetSingleIncludeAsync(p=>p.Id==id,pro=>pro.Product);
                model.ProductId = productManual.ProductId;
                model.ProductName = productManual.Product.Name;

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
                ProductManual pManual =  await _unitOfWork.Repository<ProductManual>().GetByIdAsync(id);
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
                     await _unitOfWork.Repository<ProductManual>().InsertAsync(productManual);
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
            var productList = _unitOfWork.Repository<Product>().GetAll().Where(x => x.Name.ToLower().StartsWith(term)).Select(x => new { label = x.Name, val = x.Id }).ToList();


            return Json(productList);
        }
    }
}