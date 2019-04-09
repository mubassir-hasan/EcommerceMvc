using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_MVC_Core.Models.Admin;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce_MVC_Core.Models
{
    public class ApplicationUsers:IdentityUser
    {
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string JoinIp { get; set; }
        public string Address { get; set; }
        public int? CityId { get; set; }
        public string Refference { get; set; }

        [ForeignKey("CityId")]
        public  City City { get; set; }
        public  ICollection<ProductComments> ProductCommentses { get; set; }
        public  ICollection<Orders> Orderses { get; set; }
        public  ICollection<OrderStatus> OrderStatuses { get; set; }
        //public virtual ICollection<UserLoginHistory> UserLoginHistories { get; set; }
       // public virtual ICollection<ProductLikes> ProductLikeses { get; set; }
    }
}
