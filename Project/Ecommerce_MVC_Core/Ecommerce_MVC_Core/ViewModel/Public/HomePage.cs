using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_MVC_Core.Models.Admin;

namespace Ecommerce_MVC_Core.ViewModel.Public
{
    public class HomePage
    {
        
        public List<CategoryViewModel> Categories { get; set; }
        public List<ProductListViewModel> ProductList { get; set; }
        public List<ProductListViewModel> FirstTab { get; set; }
        public List<ProductListViewModel> SecondTab { get; set; }
        public List<ProductListViewModel> ThirdTab { get; set; }
        public List<BrandListViewModel> BrandList { get; set; }
    }
}
