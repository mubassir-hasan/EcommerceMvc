﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Ecommerce_MVC_Core.Data;
using Ecommerce_MVC_Core.Models;
using Ecommerce_MVC_Core.Models.Admin;
using Ecommerce_MVC_Core.Repository;
using Ecommerce_MVC_Core.ViewModel;
using Ecommerce_MVC_Core.ViewModel.Public;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        public IActionResult Index(string search = "")
        {
            List<OrdersViewModel> orderslList = new List<OrdersViewModel>();
            orderslList = GetAllOrders();
            if (!String.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                orderslList = GetAllOrders().Where(x =>
                x.Number.ToLower().Contains(search) ||
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
            _unitOfWork.Repository<Orders>().GetAllInclude(x => x.PaymentMethod, l => l.Location).OrderByDescending(x => x.AddedDate).ToList().ForEach(o =>
                    {
                        OrdersViewModel order = new OrdersViewModel
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

        public async Task<IActionResult> Details(int id)
        {
            var model = new OrderDetailsAdminVM();
            var query = _unitOfWork.Repository<Orders>().Query();
            var order = await query
                .Include(l => l.Location)
                    .ThenInclude(c => c.City)
                        .ThenInclude(c => c.Country)
                .Include(x => x.PaymentMethod)
                .Include(x => x.Users)
                .Include(x => x.OrderDetails)
                .FirstOrDefaultAsync(x=>x.Id==id);
            if (order != null)
            {

                var productIds = order.OrderDetails.Select(x => x.ProductId);
                var productList = await _unitOfWork.Repository<Product>().Query()
                    .Include(p=>p.ProductImages)
                    .Include(x=>x.Brand)
                    .Where(x => productIds.Any(p => p == x.Id)).ToListAsync();
                model.LocationName = order.Location.Name;
                model.City = order.Location.City.Name;
                model.Country = order.Location.City.Country.Name;
                model.CustomerEmail = order.Users.Email;
                model.CustomerName = order.Users.Name;
                model.DeliveryAddress = order.DeliveryAddress;
                model.DeliveryCharge = order.DeliveryCharge;
                model.Id = order.Id;
                model.Number = order.Number;
                model.OthersCharge = order.OthersCharge;
                model.PaymentMethod = order.PaymentMethod.Name;
                foreach (var item in order.OrderDetails)
                {
                    var product = productList.FirstOrDefault(f => f.Id == item.ProductId);
                    var orderDetails = new OrderDetailsViewModel();
                    orderDetails.ProductId = item.ProductId;
                    orderDetails.Quantity = item.Quantity;

                    if (product != null)
                    {
                        orderDetails.ProductName = product.Name;
                        orderDetails.FinalPrice = item.Rate*item.Quantity;
                        orderDetails.BrandId = product.BrandId;
                        orderDetails.BrandName = product.Brand.Name;
                        orderDetails.Price = product.Price;
                        orderDetails.ImagePath = product.ProductImages.FirstOrDefault(x => x.ProductId == item.ProductId)
                            ?.ImagePath;
                        
                        model.OrderProductLists.Add(orderDetails);
                    }
                }

            }
            return View(model);
        }
    }
}