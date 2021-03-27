using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_MVC_Core.Data;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce_MVC_Core.Models.Admin
{
    public class Orders : BaseEntity
    {
        public string Number { get; set; }
        public string UserId { get; set; }
        public double Total { get; set; }
        public double DeliveryCharge { get; set; }
        public double OthersCharge { get; set; }
        public int PaymentMethodId { get; set; }
        public string DeliveryAddress { get; set; }
        public int LocationId { get; set; }

        public  ApplicationUsers Users { get; set; }
        public  PaymentMethod PaymentMethod { get; set; }
        public  Location Location { get; set; }
        public  ICollection<OrderDetails> OrderDetails { get; set; }
        public  ICollection<OrderStatus> OrderStatus { get; set; }
        
    }

    public class OrdersMap
    {
        public OrdersMap(EntityTypeBuilder<Orders> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.Number).HasMaxLength(100);
            entityTypeBuilder.Property(x => x.Total);
            entityTypeBuilder.Property(x => x.DeliveryCharge);
            entityTypeBuilder.Property(x => x.OthersCharge);
            entityTypeBuilder.Property(x => x.DeliveryAddress).HasMaxLength(200);

            entityTypeBuilder.HasOne(x => x.Users).WithMany(x => x.Orderses).HasForeignKey(x => x.UserId);
            entityTypeBuilder.HasOne(x => x.PaymentMethod).WithMany(x => x.Orderses)
                .HasForeignKey(x => x.PaymentMethodId);
            entityTypeBuilder.HasOne(x => x.Location).WithMany(x => x.Orderses).HasForeignKey(x => x.LocationId);
        }
    }
}
