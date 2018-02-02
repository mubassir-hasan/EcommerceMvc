using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce_MVC_Core.ViewModel.Public
{
    public class CommentViewModel
    {
        public string UserId { get; set; }
        [Required]
        public string Comment { get; set; }
        public int ProductId { get; set; }

    }

    public class CommentsListViewModel
    {
        public string UserId { get; set; }
        public string Comment { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string UserName { get; set; }
        public string AddedDate { get; set; }
    }
}
