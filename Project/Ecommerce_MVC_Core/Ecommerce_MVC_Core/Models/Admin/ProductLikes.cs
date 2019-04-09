using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_MVC_Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce_MVC_Core.Models.Admin
{
    public class ProductLikes
    {
        public int ProductId { get; set; }
        public string UserId { get; set; }
        public DateTime AddedDate { get; set; }

        public  Product Product { get; set; }
        public  ApplicationUsers Users { get; set; }
    }
    
    public class ProductLikesMap
    {
        public ProductLikesMap(EntityTypeBuilder<ProductLikes> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => new {x.ProductId,x.UserId});
            entityTypeBuilder.Property(x => x.AddedDate).HasDefaultValue(DateTime.Now);
            }
    }
}
