using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_MVC_Core.Data;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Ecommerce_MVC_Core.Models.Admin
{
    public class OrderDetails :BaseEntity
    {
        
        public int OrderId { get; set; }
        
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public double Rate { get; set; }
        public string Remarks { get; set; }

        public  Orders Orders { get; set; }
        public  Product Product { get; set; }
    }

    public class OrderDetailsMap
    {
        public OrderDetailsMap(EntityTypeBuilder<OrderDetails> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.Quantity);
            entityTypeBuilder.Property(x => x.Rate);
            entityTypeBuilder.Property(x => x.Remarks).HasMaxLength(200);
            entityTypeBuilder.HasOne(x => x.Product).WithMany(x => x.OrderDetails)
                .HasForeignKey(x => x.ProductId);
            entityTypeBuilder.HasOne(x => x.Orders).WithMany(x => x.OrderDetails)
                .HasForeignKey(k=>k.OrderId);
        }
    }
}
//(localdb)\\mssqllocaldb