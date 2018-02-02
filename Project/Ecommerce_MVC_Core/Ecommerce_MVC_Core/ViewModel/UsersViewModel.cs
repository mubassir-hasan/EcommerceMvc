using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce_MVC_Core.ViewModel
{
    public class UsersViewModel
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Password), MinLength(8,ErrorMessage = "Password must be 8 character")]
        public string Password { get; set; }

        [DataType(DataType.Password), MinLength(8, ErrorMessage = "Password must be 8 character")]
        [Compare("Password", ErrorMessage = "Password is not same")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Contact { get; set; }

        [Required]
        public string Gender { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Birth Day")]
        public DateTime DateOfBirth { get; set; }

        public string JoinIp { get; set; }

        
        public string Address { get; set; }

        [Display(Name = "City")]
        public int CityId { get; set; }

        public string Refference { get; set; }
        

        public List<SelectListItem> Cities { get; set; }

        [Display(Name = "Country")]
        public int CountryId { get; set; }
        public List<SelectListItem> Countries { get; set; }
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
    }

    public class UserListViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        
        public string CityName { get; set; }
        public int? CityId { get; set; }
        public string Gender { get; set; }
        public string Contact { get; set; }

    }
}
