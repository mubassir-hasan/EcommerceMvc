using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_MVC_Core.Data;

namespace Ecommerce_MVC_Core.ViewModel
{
    public class ProductStockViewModel:BaseEntity
    {
        [Display(Name = "Product")]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        [Display(Name = "Product In")]
        public int InQuantity { get; set; }
        [Display(Name = "Product Out")]
        public int OutQuantity { get; set; }
        public string Remarks { get; set; }
    }

    public class ProductStockListViewModel : BaseEntity
    {
        public string ProductName { get; set; }
        public int ProductId { get; set; }

        public int InQuantity { get; set; }
        public int OutQuantity { get; set; }

        public int InStock { get; set; }
        public string Remarks { get; set; }
    }
}
