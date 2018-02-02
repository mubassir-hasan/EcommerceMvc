using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce_MVC_Core.ViewModel.Public
{
    public class OrderDetailsViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public double FinalPrice { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
        public string ImagePath { get; set; }
        public int TotalPrice { get; set; }
    }

    public class NewOrderViewModel
    {
        public int OrderId { get; set; }
        public string DaliveryAddress { get; set; }
        public float DeliveryCharge { get; set; }
        public int LocationId { get; set; }
        public string OrderNumber { get; set; }
        public float OthersCharge { get; set; }
        public int PaymentMethodId { get; set; }
        public float Total { get; set; }
        public string UserId { get; set; }

        public int OrderDetailsId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public float Rate { get; set; }
        public string Remarks { get; set; }

        public List<SelectListItem> Countries { get; set; }
        public List<SelectListItem> Cities { get; set; }
        public List<SelectListItem> Locations { get; set; }
        public List<SelectListItem> PaymentMethods { get; set; }
        public List<OrderDetailsViewModel> OrderDetailsList { get; set; }
    }
}
