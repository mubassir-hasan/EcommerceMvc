using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_MVC_Core.Data;

namespace Ecommerce_MVC_Core.ViewModel
{
    public class ProductCommentsViewModel:BaseEntity
    {
        [Display(Name = "Product")]
        public int ProductId { get; set; }
        [Display(Name = "User")]
        public string UserId { get; set; }
        public string Comment { get; set; }
    }

    public class ProductCommentsListViewModel : BaseEntity
    {
        public string ProductName { get; set; }
        public int ProductId { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string Comment { get; set; }
        public string UserReaction { get; set; }

    }

    public class ProductCommentsListByUser
    {
        
    }



}
