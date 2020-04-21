using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce_MVC_Core.ViewModel
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }

        [Display(Name = "Category")]
        public int? CategoryId { get; set; }
        public List<SelectListItem> Categories { get; set; }
        public List<CategoryViewModel> CategoryMenus { get; set; }
    }

    public class CategoryListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        
        public int? CategoryId { get; set; }
        [Display(Name = "Category")]
        public string CategoryParentName { get; set; }
        public int TotalProduct { get; set; }
    }
}
