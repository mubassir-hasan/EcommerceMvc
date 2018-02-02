using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_MVC_Core.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce_MVC_Core.ViewModel
{
    public class ProductImageViewModel:BaseEntity
    {
        [Display(Name = "Product")]
        public int ProductId { get; set; }
        public string ImagePath { get; set; }
        public IFormFile ImageFile { get; set; }
        public string Title { get; set; }
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public List<SelectListItem> Categories { get; set; }
    }

    public class ProductImageListViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ImagePath { get; set; }
        public string Title { get; set; }
        public string ProductName { get; set; }
        
    }
}
