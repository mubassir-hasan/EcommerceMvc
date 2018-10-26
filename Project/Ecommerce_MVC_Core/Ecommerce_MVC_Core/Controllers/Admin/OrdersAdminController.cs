using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Ecommerce_MVC_Core.Data;
using Ecommerce_MVC_Core.Models;
using Ecommerce_MVC_Core.Models.Admin;
using Ecommerce_MVC_Core.Repository;
using Ecommerce_MVC_Core.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_MVC_Core.Controllers.Admin
{
    public class OrdersAdminController : Controller
    {
        private readonly UserManager<ApplicationUsers> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public OrdersAdminController(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUsers> userManager
        )
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
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
           _unitOfWork.Repository<Orders>().GetAllInclude(x=>x.PaymentMethod,l=>l.Location).OrderByDescending(x=>x.AddedDate).ToList().ForEach(o =>
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
                var user = _userManager.FindByIdAsync(o.UserId).Result;
                order.UserName = user.Name;
                order.PaymentMethod = o.PaymentMethod.Name;
                order.LocationName = o.Location.Name;
                orderslList.Add(order);
            });
            return orderslList;
        }
    }
}