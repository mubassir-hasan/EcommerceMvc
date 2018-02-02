using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce_MVC_Core.ViewModel
{
    public class LocationViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Latitude { get; set; }
        public string Longditude { get; set; }
        public string Pram { get; set; }
        [Required]
        public double Charge { get; set; }
        public int CityId { get; set; }
        public int CountryId { get; set; }
        public List<SelectListItem> Cities { get; set; }
        public List<SelectListItem> Countries { get; set; }
    }

    public class LocationListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Latitude { get; set; }
        public string Longditude { get; set; }
        public string Pram { get; set; }
       
        public double Charge { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int TotalOrders { get; set; }
    }


}
