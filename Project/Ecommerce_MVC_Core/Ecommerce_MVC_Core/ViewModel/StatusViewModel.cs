using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_MVC_Core.Data;

namespace Ecommerce_MVC_Core.ViewModel
{
    public class StatusViewModel:BaseEntity
    {
        public string Name { get; set; }
        public string Level { get; set; }
        public string Description { get; set; }
    }
}
