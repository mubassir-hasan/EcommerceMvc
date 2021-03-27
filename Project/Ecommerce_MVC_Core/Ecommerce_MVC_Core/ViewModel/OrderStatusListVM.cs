using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce_MVC_Core.ViewModel
{
    public class OrderStatusListVM
    {
        public int OrderId { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Note { get; set; }
        public DateTime Date { get; set; }
    }
}
