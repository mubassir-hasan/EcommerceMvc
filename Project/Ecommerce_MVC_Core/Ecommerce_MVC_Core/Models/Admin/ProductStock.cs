using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_MVC_Core.Data;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce_MVC_Core.Models.Admin
{
    public class ProductStock:BaseEntity
    {
        public int ProductId { get; set; }
        public int InQuantity { get; set; }
        public int OutQuantity { get; set; }
        public string Remarks { get; set; }
        
        public  Product Product { get; set; }
    }

    public class ProductStockMap
    {
        public ProductStockMap(EntityTypeBuilder<ProductStock> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.InQuantity);
            entityTypeBuilder.Property(x => x.OutQuantity);
            entityTypeBuilder.Property(x => x.Remarks).HasMaxLength(200);
            entityTypeBuilder.HasOne(x => x.Product).WithMany(x => x.ProductStocks).HasForeignKey(x => x.ProductId);
            
        }
    }
}
