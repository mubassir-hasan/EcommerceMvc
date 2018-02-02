using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Ecommerce_MVC_Core.Data;
using Ecommerce_MVC_Core.Models.Admin;
using Ecommerce_MVC_Core.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Ecommerce_MVC_Core.Controllers.Admin
{
    public class ProductStockController : Controller
    {
        private readonly IRepository<Product> _repoProduct;
        private readonly IRepository<ProductStock> _repoProductStock;

        public ProductStockController(IRepository<Product> repoProduct, IRepository<ProductStock> repoProductStock)
        {
            _repoProduct = repoProduct;
            _repoProductStock = repoProductStock;
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
        public IActionResult AddEditProductStock(int id)
        {
            ProductStockViewModel model=new ProductStockViewModel();
            if (id>0)
            {
                ProductStock productStock = _repoProductStock.GetById(id);
                model.Id = productStock.Id;
                model.InQuantity = productStock.InQuantity;
                model.OutQuantity = productStock.OutQuantity;
                model.ProductId = productStock.ProductId;
                model.Remarks = productStock.Remarks;
                model.ProductName = _repoProduct.GetAll().First(x => x.Id == productStock.ProductId).Name;
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult AddEditProductStock(int id, ProductStockViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("","Opps something wrong");
                return View(model);
            }
            if (id>0)
            {
                ProductStock productStock = _repoProductStock.GetById(id);
                productStock.InQuantity = model.InQuantity;
                productStock.OutQuantity = model.OutQuantity;
                productStock.ModifiedDate=DateTime.Now;
                productStock.ProductId = model.ProductId;
                productStock.Remarks = model.Remarks;
                _repoProductStock.Update(productStock);
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
                _repoProductStock.Insert(productStock);
            }
            return RedirectToAction(nameof(AddEditProductStock));
        }

        public List<ProductStockListViewModel> GetProductsStock()
        {
            List<ProductStockListViewModel> productList=new List<ProductStockListViewModel>();
            _repoProductStock.GetAll().ToList().ForEach(x =>
            {
                ProductStockListViewModel productStock = new ProductStockListViewModel
                {
                    Id = x.Id,
                    ProductId = x.ProductId,
                    ModifiedDate = x.ModifiedDate,
                    AddedDate = x.AddedDate,
                    InQuantity = x.InQuantity,
                    ProductName = _repoProduct.GetAll().First(p => p.Id == x.ProductId).Name,
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

            ProductStock productStock = _repoProductStock.GetById(id);
            string name = _repoProduct.GetAll().First(x => x.Id == productStock.ProductId).Name;

            return PartialView("_DeleteProductStock", name);
        }

        [HttpPost]
        public IActionResult Delete(int id, IFormCollection form)
        {
            ProductStock productStock = _repoProductStock.GetById(id);
            if (productStock != null)
            {
                _repoProductStock.Delete(productStock);
                

            }
            return RedirectToAction("Index");
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