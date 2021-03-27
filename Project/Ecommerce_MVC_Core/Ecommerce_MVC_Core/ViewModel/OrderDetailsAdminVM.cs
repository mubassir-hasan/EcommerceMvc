using Ecommerce_MVC_Core.ViewModel.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce_MVC_Core.ViewModel
{
    public class OrderDetailsAdminVM
    {
        public OrderDetailsAdminVM()
        {
            OrderProductLists = new List<OrderDetailsViewModel>();
            OrderStatusList = new List<OrderStatusListVM>();
        }
        public long Id { get; set; }
        public string Number { get; set; }
        public double DeliveryCharge { get; set; }
        public double OthersCharge { get; set; }
        public string DeliveryAddress { get; set; }
        public string LocationName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string PaymentMethod { get; set; }

        public List<OrderDetailsViewModel> OrderProductLists { get; set; }
        public List<OrderStatusListVM> OrderStatusList { get; set; }
    }
}
