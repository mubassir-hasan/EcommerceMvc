using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce_MVC_Core.ViewModel
{
    public class CityViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Display(Name = "Country")]
        public int CountryId { get; set; }
        public List<SelectListItem> Countries { get; set; }
    }

    public class CityListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        [Display(Name = "Locations")]
        public int TotalLocation { get; set; }

    }

}
