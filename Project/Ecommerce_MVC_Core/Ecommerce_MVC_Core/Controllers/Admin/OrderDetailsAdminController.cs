using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_MVC_Core.Data;
using Ecommerce_MVC_Core.Models;
using Ecommerce_MVC_Core.Models.Admin;
using Ecommerce_MVC_Core.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_MVC_Core.Controllers.Admin
{
    public class OrderDetailsAdminController : Controller
    {

        private readonly IRepository<Product> _repoProduct;
        private readonly IRepository<Orders> _repoOrders;
        private readonly IRepository<OrderDetails> _repoOrderDetails;

        public OrderDetailsAdminController(UserManager<ApplicationUsers> userManager,IRepository<Product> repoProduct, IRepository<Orders> repoOrders, IRepository<OrderDetails> repoOrderDetails)
        {
            _repoProduct = repoProduct;
            _repoOrders = repoOrders;
            _repoOrderDetails = repoOrderDetails;
        }

        public IActionResult Index( string search="")
        {
            List<OrderDetailsListViewModel> orderslList = new List<OrderDetailsListViewModel>();
            orderslList = GetAllOrderDetails();
            if (!String.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                orderslList = GetAllOrderDetails().Where(x =>
                    x.OrderNumber.ToLower().Contains(search) 
                ).ToList();
                ViewBag.SearchString = search;
            }

            return View(orderslList);
        }


        public List<OrderDetailsListViewModel> GetAllOrderDetails()
        {
            List<OrderDetailsListViewModel> orderDetailsList = new List<OrderDetailsListViewModel>();
            _repoOrderDetails.GetAll().OrderByDescending(x=>x.AddedDate).ToList().ForEach(o =>
            {
                OrderDetailsListViewModel orderDetails=new OrderDetailsListViewModel
                {
                    Id = o.Id,
                    ModifiedDate = o.ModifiedDate,
                    AddedDate = o.AddedDate,
                    ProductId = o.ProductId,
                    OrderId = o.OrderId,
                    Quantity = o.Quantity,
                    Rate = o.Rate,
                    Remarks = o.Remarks,
                };
                orderDetails.OrderNumber = _repoOrders.GetAll().First(x => x.Id == o.OrderId).Number;
                orderDetails.ProductName = _repoProduct.GetAll().First(x => x.Id == o.ProductId).Name;
                orderDetailsList.Add(orderDetails);
            });

            return orderDetailsList;
        }
    }
}