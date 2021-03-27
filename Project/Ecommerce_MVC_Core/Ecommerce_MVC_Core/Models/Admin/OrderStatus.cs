using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_MVC_Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce_MVC_Core.Models.Admin
{
    public class OrderStatus:BaseEntity
    {

        public int OrderId { get; set; }
        public int StatusId { get; set; }
        public string UserId { get; set; }
        public string Note { get; set; }

        public  Orders Orders { get; set; }
        public  Status Status { get; set; }
        public  ApplicationUsers Users { get; set; }
    }

    public class OrderStatusMap
    {
        public OrderStatusMap(EntityTypeBuilder<OrderStatus> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.AddedDate).HasDefaultValue(DateTime.Now);
            entityTypeBuilder.HasOne(x => x.Orders).WithMany(x => x.OrderStatus)
                .HasForeignKey(x => x.OrderId);
            entityTypeBuilder.HasOne(x => x.Status).WithMany(x => x.OrderStatus)
                .HasForeignKey(x => x.StatusId);
            entityTypeBuilder.HasOne(x => x.Users).WithMany(x => x.OrderStatuses).HasForeignKey(x => x.UserId);
        }
    }
}
