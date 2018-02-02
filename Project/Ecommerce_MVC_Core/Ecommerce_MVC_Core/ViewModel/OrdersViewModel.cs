using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_MVC_Core.Data;

namespace Ecommerce_MVC_Core.ViewModel
{
    public class OrdersViewModel:BaseEntity
    {
        public string Number { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public double Total { get; set; }
        public double DeliveryCharge { get; set; }
        public double OthersCharge { get; set; }
        public string PaymentMethod { get; set; }
        public int PaymentMethodId { get; set; }
        public string DeliveryAddress { get; set; }
        public string LocationName { get; set; }
        public int LocationId { get; set; }
    }
}
