using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_MVC_Core.Data;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce_MVC_Core.Models.Admin
{
    public class ProductComments:BaseEntity
    {
        public int ProductId { get; set; }
        public string UserId { get; set; }
        public string Comment { get; set; }

        public  Product Product { get; set; }
        public  ApplicationUsers Users { get; set; }
    }

    public class ProductCommentsMap
    {
        public ProductCommentsMap(EntityTypeBuilder<ProductComments> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.Comment).HasMaxLength(200);
            entityTypeBuilder.HasOne(x => x.Product).WithMany(x => x.ProductCommentses).HasForeignKey(x => x.ProductId);
            entityTypeBuilder.HasOne(x => x.Users).WithMany(x => x.ProductCommentses).HasForeignKey(x => x.UserId);
        }
    }

}
