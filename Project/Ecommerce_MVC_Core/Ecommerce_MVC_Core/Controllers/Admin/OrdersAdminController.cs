using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Ecommerce_MVC_Core.Data;
using Ecommerce_MVC_Core.Models;
using Ecommerce_MVC_Core.Models.Admin;
using Ecommerce_MVC_Core.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_MVC_Core.Controllers.Admin
{
    public class OrdersAdminController : Controller
    {
        private readonly UserManager<ApplicationUsers> _userManager;
        private readonly IRepository<PaymentMethod> _repoPaymentMethod;
        private readonly IRepository<Location> _repoLocation;
        private readonly IRepository<Orders> _repoOrders;

        public OrdersAdminController(IRepository<Orders> repoOrders,UserManager<ApplicationUsers> userManager, IRepository<PaymentMethod> repoPaymentMethod, IRepository<Location> repoLocation)
        {
            _userManager = userManager;
            _repoPaymentMethod = repoPaymentMethod;
            _repoLocation = repoLocation;
            _repoOrders = repoOrders;
        }

        public IActionResult Index(string search="")
        {
            List<OrdersViewModel> orderslList = new List<OrdersViewModel>();
            orderslList = GetAllOrders();
            if (!String.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                orderslList = GetAllOrders().Where(x => 
                x.Number.ToLower().Contains(search)||
                x.LocationName.ToLower().Contains(search) ||
                x.UserName.ToLower().Contains(search) ||
                x.PaymentMethod.ToLower().Contains(search) 
                ).ToList();
                ViewBag.SearchString = search;
            }
                
            return View(orderslList);
        }

        public List<OrdersViewModel> GetAllOrders()
        {
            List<OrdersViewModel> orderslList = new List<OrdersViewModel>();
            _repoOrders.GetAll().OrderByDescending(x=>x.AddedDate).ToList().ForEach(o =>
            {
                OrdersViewModel order=new OrdersViewModel
                {
                    Id = o.Id,
                    ModifiedDate = o.ModifiedDate,
                    AddedDate = o.AddedDate,
                    DeliveryAddress = o.DeliveryAddress,
                    DeliveryCharge = o.DeliveryCharge,
                    LocationId = o.LocationId,
                    Number = o.Number,
                    OthersCharge = o.OthersCharge,
                    PaymentMethodId = o.PaymentMethodId,
                    Total = o.Total,
                    UserId = o.UserId,
                };
                ApplicationUsers user = _userManager.FindByIdAsync(o.UserId).Result;
                order.UserName = user.Name;
                order.PaymentMethod = _repoPaymentMethod.GetAll().First(x => x.Id == o.PaymentMethodId).Name;
                order.LocationName = _repoLocation.GetAll().First(x => x.Id == o.LocationId).Name;
                orderslList.Add(order);
            });
            return orderslList;
        }
    }
}