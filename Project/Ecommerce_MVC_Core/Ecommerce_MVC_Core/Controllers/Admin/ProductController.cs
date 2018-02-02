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
    public class ProductController : Controller
    {
        private readonly IRepository<Product> _repoProducts;
        private readonly IRepository<Category> _repoCategory;
        private readonly IRepository<Brand> _repoBrand;
        private readonly IRepository<Unit> _repoUnit;
        private readonly IRepository<ProductComments> _repoProductComment;
        private readonly IRepository<ProductStock> _repoProductStock;
        private readonly IRepository<ProductImage> _repoProductImage;

        public ProductController(IRepository<Product> repoProducts, 
            IRepository<Category> repoCategory, 
            IRepository<Brand> repoBrand, 
            IRepository<Unit> repoUnit, 
            IRepository<ProductComments> repoProductComment,
            IRepository<ProductStock> repoProductStock,
            IRepository<ProductImage> repoProductImage)
        {
            _repoProducts = repoProducts;
            _repoCategory = repoCategory;
            _repoBrand = repoBrand;
            _repoUnit = repoUnit;
            _repoProductComment = repoProductComment;
            _repoProductStock = repoProductStock;
            _repoProductImage = repoProductImage;
        }

        public IActionResult Index(string search)
        {
            List<ProductListViewModel> model = new List<ProductListViewModel>();

            if (!String.IsNullOrEmpty(search))
            {
                _repoProducts.GetAll().Where(x => 
                x.Name.ToLower().Contains(search.ToLower()) ||
                x.Code.ToLower().Contains(search.ToLower()) ||
                x.Tag.ToLower().Contains(search.ToLower()) 

                ).ToList().ForEach(b =>
                {
                    ProductListViewModel product = new ProductListViewModel
                    {
                        Id = b.Id,
                        Name = b.Name,
                        Code = b.Code,
                        Tag = b.Tag,
                        CategoryId = b.CategoryId,
                        BrandId = b.BrandId,
                        UnitId = b.UnitId,
                        Description = b.Description,
                        Price = b.Price,
                        BrandName = _repoBrand.GetAll().First(x=>x.Id==b.BrandId).Name,
                        CategoryName = _repoCategory.GetAll().First(x => x.Id == b.CategoryId).Name,
                        UnitName = _repoUnit.GetAll().First(x => x.Id == b.UnitId).Name,
                        Discount = b.Discount,
                        FinalPrice = (b.Price-((b.Price * b.Discount) /100)),
                        ProductComments = _repoProductComment.GetAll().Count(x=>x.ProductId==b.Id),
                        TotalImage = _repoProductImage.GetAll().Count(x => x.ProductId == b.Id)
                    };
                    var prdctStocks = _repoProductStock.GetAll().FirstOrDefault(x => x.ProductId == b.Id);
                    product.ProductStocks = prdctStocks != null ? product.ProductStocks = prdctStocks.InQuantity - prdctStocks.OutQuantity : product.ProductStocks = 0; 
                    ViewBag.SearchString = search;
                    model.Add(product);
                });

            }
            else
            {
                _repoProducts.GetAll().ToList().ForEach(b =>
                {
                    ProductListViewModel product = new ProductListViewModel
                    {
                        Id = b.Id,
                        Name = b.Name,
                        Code = b.Code,
                        Tag = b.Tag,
                        CategoryId = b.CategoryId,
                        BrandId = b.BrandId,
                        UnitId = b.UnitId,
                        Description = b.Description,
                        Price = b.Price,
                        BrandName = _repoBrand.GetAll().First(x => x.Id == b.BrandId).Name,
                        CategoryName = _repoCategory.GetAll().First(x => x.Id == b.CategoryId).Name,
                        UnitName = _repoUnit.GetAll().First(x => x.Id == b.UnitId).Name,
                        Discount = b.Discount,
                        FinalPrice = (b.Price - ((b.Price * b.Discount) / 100)),
                        ProductComments = _repoProductComment.GetAll().Count(x => x.ProductId == b.Id),
                        TotalImage = _repoProductImage.GetAll().Count(x=>x.ProductId==b.Id)
                    };
                    var prdctStocks = _repoProductStock.GetAll().FirstOrDefault(x => x.ProductId == b.Id);
                    product.ProductStocks = prdctStocks!=null? product.ProductStocks = prdctStocks.InQuantity - prdctStocks.OutQuantity:product.ProductStocks=0;

                    model.Add(product);
                });
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult AddEditProduct(int id)
        {
            ProductViewModel model = new ProductViewModel();
            model.BrandList = _repoBrand.GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            model.CategoryList = _repoCategory.GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            model.UnitList = _repoUnit.GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            if (id > 0)
            {
                Product product = _repoProducts.GetById(id);
                model.Name = product.Name;
                model.Code = product.Code;
                model.Tag = product.Tag;
                model.CategoryId = product.CategoryId;
                model.BrandId = product.BrandId;
                model.UnitId = product.UnitId;
                model.Description = product.Description;
                model.Price = product.Price;
                model.Discount = product.Discount;
            }

            return PartialView("_AddEditProduct", model);
        }

        [HttpPost]
        public IActionResult AddEditProduct(int id, ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("","Something Wrong");
                return View("_AddEditProduct", model);
            }
            if (id>0)
            {
                Product product = _repoProducts.GetById(id);
                if (product!=null)
                {
                    product.Name = model.Name;
                    product.Code = model.Code;
                    product.Tag = model.Tag;
                    product.CategoryId = model.CategoryId;
                    product.BrandId = model.BrandId;
                    product.UnitId = model.UnitId;
                    product.Description = model.Description;
                    product.Price = model.Price;
                    product.Discount = model.Discount;

                    product.ModifiedDate = DateTime.Now;
                    _repoProducts.Update(product);
                }
                
            }
            else
            {
                Product product = new Product
                {
                    Name = model.Name,
                    Code = model.Code,
                    Tag = model.Tag,
                    CategoryId = model.CategoryId,
                    BrandId = model.BrandId,
                    UnitId = model.UnitId,
                    Description = model.Description,
                    Price = model.Price,
                    Discount = model.Discount,
                    AddedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                };
                _repoProducts.Insert(product);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult DeleteProduct(int id)
        {
            Product product = _repoProducts.GetById(id);

            return PartialView("_DeleteProduct", product?.Name);
        }

        [HttpPost]
        public IActionResult DeleteProduct(int id, IFormCollection form)
        {
            Product product = _repoProducts.GetById(id);
            if (product != null)
            {
                _repoProducts.Delete(product);
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ListImageView(int id)
        {
            ProductImageListByProduct productImage=new ProductImageListByProduct();
            //productImage.ProuctImages = GetProdutcsImages(id);
            productImage.Path = productImage.ProuctImages.Max(x => x.ImagePath);

            //return PartialView("_ShowImageByProduct", productImage);
            return View();
        }

        public PartialViewResult GetProdutcsImages(int id)
        {
            List<ProductImageListViewModel> productImageList = new List<ProductImageListViewModel>();
            ViewBag.productName = _repoProducts.GetById(id).Name;
            _repoProductImage.GetAll().Where(x => x.ProductId == id).ToList().ForEach(x =>
            {
                ProductImageListViewModel pImage = new ProductImageListViewModel
                {
                    Id = x.Id,
                    ProductId = x.ProductId,
                    ImagePath = x.ImagePath,
                    Title = x.Title,
                    ProductName = _repoProducts.GetById(id).Name
                };
                productImageList.Add(pImage);
            });
            return PartialView("_ShowImageByProduct", productImageList);
            
        }
    }
}