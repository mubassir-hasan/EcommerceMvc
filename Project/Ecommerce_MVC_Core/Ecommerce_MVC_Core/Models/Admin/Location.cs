using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_MVC_Core.Data;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce_MVC_Core.Models.Admin
{
    public class Location : BaseEntity
    {
        public string Name { get; set; }
        public string Latitude { get; set; }
        public string Longditude { get; set; }
        public string Pram { get; set; }
        public double Charge { get; set; }
        public int CityId { get; set; }

        public  City City { get; set; }
        public  ICollection<Orders> Orderses { get; set; }
    }

    public class LocationMap
    {
        public LocationMap(EntityTypeBuilder<Location> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.Name).HasMaxLength(200);
            entityTypeBuilder.Property(x => x.Latitude).HasMaxLength(100);
            entityTypeBuilder.Property(x => x.Longditude).HasMaxLength(100);
            entityTypeBuilder.Property(x => x.Pram).HasMaxLength(200);
            entityTypeBuilder.Property(x => x.Charge);
            entityTypeBuilder.HasOne(x => x.City).WithMany(x => x.Locations).HasForeignKey(x => x.CityId);
        }
    }
}
