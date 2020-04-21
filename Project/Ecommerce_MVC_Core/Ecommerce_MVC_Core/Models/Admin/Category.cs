using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_MVC_Core.Data;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Ecommerce_MVC_Core.Models.Admin
{
    public class Category:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        
        public int?  CategoryId { get; set; }

        //[ForeignKey("CategoryId")]
        public Category Categoris { get; set; }
        public ICollection<Category> CategoriesList { get; set; } 
        public ICollection<Product> Products { get; set; }
    }

    public class CategoryMap
    {
        public CategoryMap(EntityTypeBuilder<Category> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.Name);
            entityTypeBuilder.Property(x => x.Description);
            entityTypeBuilder.HasOne(x => x.Categoris).WithMany(x => x.CategoriesList).HasForeignKey(x => x.CategoryId).IsRequired(false);
        }

    }
}
