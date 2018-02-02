using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_MVC_Core.Data;
using Microsoft.AspNetCore.Http;

namespace Ecommerce_MVC_Core.ViewModel
{
    public class ProductMenualViewModel:BaseEntity
    {
        [Display(Name = "Product")]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public IFormFile ProductFile { get; set; }
        [Display(Name = "Manual Name")]
        public string ManualName { get; set; }
    }
}
