using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_MVC_Core.Data;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce_MVC_Core.Models.Admin
{
    public class Country:BaseEntity
    {
        public string Name { get; set; }

        public  ICollection<City> Cities { get; set; }
    }

    public class CountryMap
    {
        public CountryMap(EntityTypeBuilder<Country> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.Name);
        }
    }
}
