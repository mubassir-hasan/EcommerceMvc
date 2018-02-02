using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_MVC_Core.Data;

namespace Ecommerce_MVC_Core.ViewModel
{
    public class OrderDetailsListViewModel:BaseEntity
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; }

        public string ProductName { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public double Rate { get; set; }
        public string Remarks { get; set; }
    }
}
