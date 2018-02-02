using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_MVC_Core.Data;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce_MVC_Core.Models.Admin
{
    public class Product:BaseEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Tag { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public int UnitId { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Discount { get; set; }

        public virtual Category Category { get; set; }
        public virtual Brand Brand { get; set; }
        public virtual Unit Unit { get; set; }
        public virtual ProductManual ProductManual { get; set; }
        public virtual ICollection<ProductComments> ProductCommentses { get; set; }
        public virtual ICollection<ProductImage> ProductImages { get; set; }
        public virtual ICollection<ProductStock> ProductStocks { get; set; }
        public virtual OrderDetails OrderDetails { get; set; }
    }

    public class ProductMap
    {
        public ProductMap(EntityTypeBuilder<Product> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.Name).HasMaxLength(100);
            entityTypeBuilder.Property(x => x.Code).HasMaxLength(100);
            entityTypeBuilder.Property(x => x.Tag).HasMaxLength(200);
            entityTypeBuilder.Property(x => x.Description);
            entityTypeBuilder.Property(x => x.Price);
            entityTypeBuilder.Property(x => x.Discount);

            entityTypeBuilder.HasOne(x => x.Category).WithMany(x => x.Products).HasForeignKey(x => x.CategoryId);
            entityTypeBuilder.HasOne(x => x.Brand).WithMany(x => x.Products).HasForeignKey(x => x.BrandId);
            entityTypeBuilder.HasOne(x => x.Unit).WithMany(x => x.Products).HasForeignKey(x => x.UnitId);

        }
    }
}
