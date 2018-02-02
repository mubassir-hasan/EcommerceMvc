using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce_MVC_Core.Models.Admin
{
    public class UsersActive
    {
        public string Email { get; set; }//PK
        public DateTime AddedDate { get; set; }
        public string IpAddress { get; set; }
    }
}
