using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_MVC_Core.ViewModel.Public;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce_MVC_Core.ViewModel
{
    public class ProductViewModel
    {
        public ProductViewModel()
        {
            Images = new List<IFormFile>();
            CategoryList = new List<SelectListItem>();
            BrandList = new List<SelectListItem>();
            UnitList = new List<SelectListItem>();
        }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Tag { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int BrandId { get; set; }
        [Required]
        public int UnitId { get; set; }
        public string Description { get; set; }
        [Required]
        public double Price { get; set; }
        public int Discount { get; set; }
        public List<SelectListItem> CategoryList { get; set; }
        public List<SelectListItem> BrandList { get; set; }
        public List<SelectListItem> UnitList { get; set; }

        //Image Upload
        public IList<IFormFile> Images { get; set; }
        public IFormFile Manual { get; set; }
    }

    public class ProductListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Tag { get; set; }
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
        public string BrandName { get; set; }
        public int BrandId { get; set; }
        public string UnitName { get; set; }
        public int UnitId { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Discount { get; set; }
        public double FinalPrice { get; set; }
        public int ProductComments { get; set; }
        public int ProductStocks { get; set; }
        public int TotalImage { get; set; }

        //For HomePage
        public string ImageTitle { get; set; }
        public string ImagePath { get; set; }
        public List<ProductImageListViewModel> ImageList { get; set; }
        public string SecondImagePath { get; set; }
        public List<CommentsListViewModel> ProductCommentsList { get; set; }
    }

    public class ProductImageListByProduct
    {
        public string Path { get; set; }
        public List<ProductImageListViewModel> ProuctImages { get; set; }
    }
}
