using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography.X509Certificates;
using Ecommerce_MVC_Core.Code;
using Ecommerce_MVC_Core.Models;
using Ecommerce_MVC_Core.Models.Admin;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_MVC_Core.Data
{


    public class ApplicationDbContext: IdentityDbContext<ApplicationUsers, ApplicationRoles, string>
    {
        

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options){
            
        }
        public  DbSet<Brand> Brands { get; set; }
        public  DbSet<Category> Categories { get; set; }
        public  DbSet<Country> Countries { get; set; }
        public  DbSet<City> Cities { get; set; }
        public  DbSet<Location> Location { get; set; }
        //public virtual DbSet<Users> Userses { get; set; }
        public  DbSet<PaymentMethod> PaymentMethod { get; set; }
        public  DbSet<Unit> Unit { get; set; }
        public  DbSet<Product> Product { get; set; }
        public  DbSet<Status> Status { get; set; }
        public  DbSet<ProductComments> ProductComments { get; set; }
        public  DbSet<ProductImage> ProductImage { get; set; }
        //public virtual DbSet<ProductLikes> ProductLikes { get; set; }
        public  DbSet<ProductManual> ProductManual { get; set; }
        public  DbSet<ProductStock> ProductStock { get; set; }

        public  DbSet<Orders> Order { get; set; }
        public  DbSet<OrderDetails> OrderDetails { get; set; }
        public  DbSet<OrderStatus> OrderStatus { get; set; }

        //ModelBuilder
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Brand>().ToTable("Brand");
            modelBuilder.Entity<Category>().ToTable("Category");
            modelBuilder.Entity<Country>().ToTable("Country");
            modelBuilder.Entity<City>().ToTable("City");
            modelBuilder.Entity<Users>().ToTable("Users");

            modelBuilder.Entity<ApplicationUsers>().HasOne(x => x.City).WithMany(x => x.Users)
                .HasForeignKey(x => x.CityId);

            //Maping tabale
            new BrandMap(modelBuilder.Entity<Brand>());
            new CategoryMap(modelBuilder.Entity<Category>());
            new CountryMap(modelBuilder.Entity<Country>());
            new CityMap(modelBuilder.Entity<City>());
            new LocationMap(modelBuilder.Entity<Location>());
            //new UsersMap(modelBuilder.Entity<Users>());
            new PaymentMethodMap(modelBuilder.Entity<PaymentMethod>());
            new UnitMap(modelBuilder.Entity<Unit>());
            new ProductMap(modelBuilder.Entity<Product>());
            new StatusMap(modelBuilder.Entity<Status>());
            new ProductCommentsMap(modelBuilder.Entity<ProductComments>());
            new ProductImageMap(modelBuilder.Entity<ProductImage>());
            new ProductLikesMap(modelBuilder.Entity<ProductLikes>());
            new ProductManualMap(modelBuilder.Entity<ProductManual>());
            new ProductStockMap(modelBuilder.Entity<ProductStock>());

            new OrdersMap(modelBuilder.Entity<Orders>());
            new OrderDetailsMap(modelBuilder.Entity<OrderDetails>());
            new OrderStatusMap(modelBuilder.Entity<OrderStatus>());

            //new UserLoginHistoryMap(modelBuilder.Entity<UserLoginHistory>());

        }

        




    }
}
