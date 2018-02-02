using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce_MVC_Core.ViewModel
{
    public class CountryViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        
    }

    public class CountryListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [Display(Name = "Total City")]
        public int TotalCities { get; set; } = 0;
    }
}
