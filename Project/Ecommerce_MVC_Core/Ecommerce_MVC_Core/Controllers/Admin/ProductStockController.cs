using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Ecommerce_MVC_Core.Data;
using Ecommerce_MVC_Core.Models.Admin;
using Ecommerce_MVC_Core.Repository;
using Ecommerce_MVC_Core.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Ecommerce_MVC_Core.Controllers.Admin
{
    public class ProductStockController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductStockController(
            IUnitOfWork unitOfWork
        )
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index(string search="")
        {
            List < ProductStockListViewModel > model;
            if (!String.IsNullOrEmpty(search))
            {
                search=search.ToLower();
                model = GetProductsStock().Where(x => x.ProductName.ToLower().Contains(search)).ToList();
                ViewBag.SearchString = search;
            }

            else
            {
                model=GetProductsStock();
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddEditProductStock(int id)
        {
            ProductStockViewModel model=new ProductStockViewModel();
            if (id>0)
            {
                ProductStock productStock = await _unitOfWork.Repository<ProductStock>().GetSingleIncludeAsync(x=>x.Id==id,p=>p.Product);
                model.Id = productStock.Id;
                model.InQuantity = productStock.InQuantity;
                model.OutQuantity = productStock.OutQuantity;
                model.ProductId = productStock.ProductId;
                model.Remarks = productStock.Remarks;
                model.ProductName = productStock.Product.Name;
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddEditProductStock(int id, ProductStockViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("","Opps something wrong");
                return View(model);
            }
            if (id>0)
            {
                ProductStock productStock = await _unitOfWork.Repository<ProductStock>().GetByIdAsync(id);
                productStock.InQuantity = model.InQuantity;
                productStock.OutQuantity = model.OutQuantity;
                productStock.ModifiedDate=DateTime.Now;
                productStock.ProductId = model.ProductId;
                productStock.Remarks = model.Remarks;
                await _unitOfWork.Repository<ProductStock>().UpdateAsync(productStock);
            }
            else
            {
                ProductStock productStock = new ProductStock
                {
                    InQuantity = model.InQuantity,
                    OutQuantity = model.OutQuantity,
                    AddedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    ProductId = model.ProductId,
                    Remarks = model.Remarks
                };
                await _unitOfWork.Repository<ProductStock>().InsertAsync(productStock);
            }
            return RedirectToAction(nameof(AddEditProductStock));
        }

        public List<ProductStockListViewModel> GetProductsStock()
        {
            List<ProductStockListViewModel> productList=new List<ProductStockListViewModel>();
             _unitOfWork.Repository<ProductStock>().GetAllInclude(x=>x.Product).ToList().ForEach(x =>
            {
                ProductStockListViewModel productStock = new ProductStockListViewModel
                {
                    Id = x.Id,
                    ProductId = x.ProductId,
                    ModifiedDate = x.ModifiedDate,
                    AddedDate = x.AddedDate,
                    InQuantity = x.InQuantity,
                    ProductName = x.Product.Name,
                    OutQuantity = x.OutQuantity,
                    Remarks = x.Remarks,
                    InStock = x.InQuantity - x.OutQuantity
                };
                productList.Add(productStock);
            });
            return productList;
        }


        [HttpGet]
        public IActionResult Delete(int id)
        {

            var productStock =  _unitOfWork.Repository<ProductStock>().GetSingleInclude(x=>x.Id==id,p=>p.Product);
            string name = productStock.Product.Name;

            return PartialView("_DeleteProductStock", name);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, IFormCollection form)
        {
            ProductStock productStock = await _unitOfWork.Repository<ProductStock>().GetByIdAsync(id);
            if (productStock != null)
            {
                await _unitOfWork.Repository<ProductStock>().DeleteAsync(productStock);
                

            }
            return RedirectToAction("Index");
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