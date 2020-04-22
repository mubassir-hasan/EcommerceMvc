using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_MVC_Core.Code;
using Ecommerce_MVC_Core.Data;
using Ecommerce_MVC_Core.Models;
using Ecommerce_MVC_Core.Models.Admin;
using Ecommerce_MVC_Core.Repository;
using Ecommerce_MVC_Core.ViewModel.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_MVC_Core.Controllers
{
    public class OrderController : Controller
    {
        //private readonly IRepository<Category> _repoCategory;
        //private readonly IRepository<Product> _repoProduct;
        //private readonly IRepository<Brand> _repoBrand;
        //private readonly IRepository<Unit> _repoUnit;

        //private readonly IRepository<ProductStock> _repoProductStock;
        //private readonly IRepository<ProductImage> _repoProductImage;
        //private readonly IRepository<Orders> _repoOrders;
        //private readonly IRepository<OrderDetails> _repoOrderDetails;
        //private readonly IRepository<Country> _repoCountry;
        //private readonly IRepository<City> _repoCity;
        //private readonly IRepository<Location> _repoLocation;
        //private readonly IRepository<PaymentMethod> _repoPaymentMethod;
        private readonly UserManager<ApplicationUsers> _userManager;

        private readonly IUnitOfWork _unitOfWork;


        public OrderController(IUnitOfWork unitOfWork, UserManager<ApplicationUsers> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }


        public IActionResult Index()
        {
            List<OrderDetailsViewModel> model = new List<OrderDetailsViewModel>();
            List<AddToCartViewModel> addToCartList = HttpContext.Session.Get<List<AddToCartViewModel>>("CartItem");
            int itemInCart = 0;
            var valu = HttpContext.Session.GetInt32("itemCount");
            if (valu != null)
            {
                itemInCart = valu.Value;
            }

            var dbProducts = _unitOfWork.Repository<Product>().Query()
                .Include(b => b.Brand)
                .Include( p => p.ProductImages)
                .Include(ps => ps.ProductStocks);
            if (addToCartList!=null)
            {
                addToCartList.ForEach(c =>
                {
                    OrderDetailsViewModel orderDetails=new OrderDetailsViewModel();
                    orderDetails.ProductId = c.ProductId;
                    orderDetails.ProductName = c.ProductName;
                    orderDetails.FinalPrice = c.FinalPrice;
                    orderDetails.Quantity = c.Quantity;
                    var product = dbProducts.FirstOrDefault(x => x.Id == c.ProductId);

                    if (product!=null)
                    {
                        orderDetails.BrandId = product.BrandId;
                        orderDetails.BrandName = product.Brand.Name;
                        orderDetails.Price = product.Price;
                        orderDetails.ImagePath =product.ProductImages.FirstOrDefault(x => x.ProductId == c.ProductId)
                            ?.ImagePath;
                        var pStock = product.ProductStocks.FirstOrDefault(x => x.ProductId == c.ProductId);
                        if (pStock!=null)
                        {
                            orderDetails.Stock = (pStock.InQuantity - pStock.OutQuantity) < 1
                                ? 0
                                : pStock.InQuantity - pStock.OutQuantity;
                        }
                        model.Add(orderDetails);
                    }
                });
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Index(IEnumerable<OrderDetailsViewModel> product)
        {
            List<AddToCartViewModel> addToCartList = HttpContext.Session.Get<List<AddToCartViewModel>>("CartItem");
            
            List <AddToCartViewModel> modelCart=new List<AddToCartViewModel>();

            int itemInCart = 0;
            double estimatePrice = 0;
            double totalPrice = 0;
            var valu = HttpContext.Session.GetInt32("itemCount");
            if (valu != null)
            {
                itemInCart = valu.Value;
            }
            HttpContext.Session.Set<List<AddToCartViewModel>>("CartItem", modelCart);
            foreach (var item in product)
            {
                AddToCartViewModel cart = new AddToCartViewModel();
                cart.ProductId = item.ProductId;
                cart.FinalPrice = item.FinalPrice;
                cart.ProductName = item.ProductName;
                cart.Quantity = item.Quantity;
                estimatePrice = item.Quantity * item.FinalPrice;
                totalPrice = totalPrice + estimatePrice;
                modelCart.Add(cart);
                
            }
            HttpContext.Session.Set<List<AddToCartViewModel>>("CartItem", modelCart);
            itemInCart = HttpContext.Session.Get<List<AddToCartViewModel>>("CartItem").Count;
            HttpContext.Session.SetInt32("itemCount", itemInCart);
            HttpContext.Session.SetInt32("TotalPrice",Convert.ToInt32(totalPrice));

            return RedirectToAction(nameof(NewOrder));
        }

        #region AddNewOrders
        [Authorize]
        public IActionResult NewOrder()
        {
            NewOrderViewModel model=new NewOrderViewModel();
            model.OrderDetailsList = GetItemOrderDetails();

            model.Countries  = _unitOfWork.Repository<Country>().GetAll().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()

            }).OrderBy(x => x.Text).ToList();

            model.Countries.Add(new SelectListItem { Text = "--Select--", Value = "0", Selected = true });
            model.Cities = new List<SelectListItem>();
            model.Locations=new List<SelectListItem>();
            model.PaymentMethods = _unitOfWork.Repository<PaymentMethod>().GetAll().Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.Id.ToString()

            }).OrderBy(s => s.Text).ToList();
            model.PaymentMethods.Add(new SelectListItem { Text = "--Select--", Value = "0",Selected = true});

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult NewOrder(NewOrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("","Something wrong");
                return View(model);
            }
            List<AddToCartViewModel> addToCartList = HttpContext.Session.Get<List<AddToCartViewModel>>("CartItem");

            if (addToCartList==null)
            {
                return RedirectToAction("Index", "Home");
            }

            var num = HttpContext.Session.GetInt32("TotalPrice");
            double totalPrice=0;
            if (num != null)
            {
                totalPrice =  Convert.ToDouble(num);
            }
            ApplicationUsers user = _userManager.GetUserAsync(HttpContext.User).Result;
            //Inserting Order
            Orders orders=new Orders
            {
                AddedDate = DateTime.Now,
                DeliveryAddress = model.DaliveryAddress,
                LocationId = model.LocationId,
                ModifiedDate = DateTime.Now,
                Number = GenerateRandomNo().ToString(),
                PaymentMethodId = model.PaymentMethodId,
                Total = totalPrice,
            };

            orders.UserId = user.Id;
            orders.DeliveryCharge = _unitOfWork.Repository<Location>().Find(x => x.Id == model.LocationId).Charge;
            _unitOfWork.Repository<Orders>().Insert(orders);
            
            //Inserting Order Details

            int oId=orders.Id;


            for (int i = 0; i < addToCartList.Count; i++)
            {

                OrderDetails orderDetails = new OrderDetails();

                orderDetails.AddedDate = DateTime.Now;
                orderDetails.ModifiedDate = DateTime.Now;
                orderDetails.OrderId = oId;
                orderDetails.ProductId = addToCartList[i].ProductId;
                orderDetails.Quantity = addToCartList[i].Quantity;
                orderDetails.Rate = addToCartList[i].FinalPrice;
                orderDetails.Remarks = "";

                _unitOfWork.Repository<OrderDetails>().Insert(orderDetails);
                    
             }
            

            addToCartList=new List<AddToCartViewModel>();
            HttpContext.Session.Set<List<AddToCartViewModel>>("CartItem", addToCartList);

           int itemInCart = HttpContext.Session.Get<List<AddToCartViewModel>>("CartItem").Count;
            HttpContext.Session.SetInt32("itemCount", itemInCart);
            HttpContext.Session.SetInt32("TotalPrice",0);

            return RedirectToAction("Index","Home");
        }

        public List<OrderDetailsViewModel> GetItemOrderDetails()
        {
            List<OrderDetailsViewModel> model = new List<OrderDetailsViewModel>();
            List<AddToCartViewModel> addToCartList = HttpContext.Session.Get<List<AddToCartViewModel>>("CartItem");
            int itemInCart = 0;
            var valu = HttpContext.Session.GetInt32("itemCount");
            if (valu != null)
            {
                itemInCart = valu.Value;
            }


            if (addToCartList != null)
            {
                var dbProducts = _unitOfWork.Repository<Product>().Query()
                    .Include(b => b.Brand)
                    .Include(p => p.ProductImages)
                    .Include(ps => ps.ProductStocks);

                addToCartList.ForEach(c =>
                {
                    OrderDetailsViewModel orderDetails = new OrderDetailsViewModel();
                    orderDetails.ProductId = c.ProductId;
                    orderDetails.ProductName = c.ProductName;
                    orderDetails.FinalPrice = c.FinalPrice;
                    orderDetails.Quantity = c.Quantity;
                    var product = dbProducts.FirstOrDefault(x => x.Id == c.ProductId);
                    if (product!=null)
                    {
                        orderDetails.BrandId = product.BrandId;
                        orderDetails.BrandName = product.Brand.Name;
                        orderDetails.Price = product.Price;
                        orderDetails.ImagePath = product.ProductImages.FirstOrDefault(x => x.ProductId == c.ProductId)
                            ?.ImagePath;
                        var pStock = product.ProductStocks.FirstOrDefault(x => x.ProductId == c.ProductId);
                        if (pStock != null)
                        {
                            orderDetails.Stock = (pStock.InQuantity - pStock.OutQuantity) < 1
                                ? 0
                                : pStock.InQuantity - pStock.OutQuantity;
                        }
                        model.Add(orderDetails);
                    }
                });
            }
            return model;
        }
        public int GenerateRandomNo()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }
        #endregion

        #region RemoveCartItem


        [HttpGet]
        public IActionResult RemoveItemFromCart(int product)
        {
            AddToCartViewModel model=new AddToCartViewModel();

            List<AddToCartViewModel> addToCartList = HttpContext.Session.Get<List<AddToCartViewModel>>("CartItem");
            //string name = "";
            model =addToCartList.FirstOrDefault(x => x.ProductId == product);
            if (model != null)
            {
                HttpContext.Session.SetInt32("ProductId",product);
            }

            return PartialView("_RemoveItemFromCart", model);
        }


        [HttpPost]
        public IActionResult RemoveItemFromCart(int? product, AddToCartViewModel model)
        {
           
                product = HttpContext.Session.GetInt32("ProductId");
            
            List<AddToCartViewModel> addToCartList = HttpContext.Session.Get<List<AddToCartViewModel>>("CartItem");
            int itemInCart = 0;
            var valu = HttpContext.Session.GetInt32("itemCount");
            if (valu != null)
            {
                itemInCart = valu.Value;
            }

            if (addToCartList.Where(x => x.ProductId == product).ToList().Count > 0)
            {
                AddToCartViewModel cart = addToCartList.FirstOrDefault(x => x.ProductId==product);
                if (cart!=null)
                {
                    addToCartList.Remove(cart);
                }
                HttpContext.Session.Set<List<AddToCartViewModel>>("CartItem", addToCartList);
                itemInCart = HttpContext.Session.Get<List<AddToCartViewModel>>("CartItem").Count;
            }
            HttpContext.Session.SetInt32("itemCount", itemInCart);
            
            return RedirectToAction("Index");

            
        }
        #endregion
    }
}