using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_MVC_Core.Data;

namespace Ecommerce_MVC_Core.ViewModel
{
    public class BrandViewModel:BaseEntity
    {
        [Required]

        public string Name { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }

    public class BrandListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [Display(Name = "Total Product")]
        public int TotalProduct { get; set; }
    }

    
}
