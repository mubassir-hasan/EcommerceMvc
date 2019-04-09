using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_MVC_Core.Data;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce_MVC_Core.Models.Admin
{
    public class Users:BaseEntity
    {
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string JoinIp { get; set; }
        public string UserType { get; set; }
        public string Address { get; set; }
        public int CityId { get; set; }

        public  City City { get; set; }
        public  ICollection<ProductComments> ProductCommentses { get; set; }
        public  ICollection<UserLoginHistory> UserLoginHistories { get; set; }
       }

    public class UsersMap
    {
        public UsersMap(EntityTypeBuilder<Users> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.Name).HasMaxLength(200);
            entityTypeBuilder.Property(x => x.Contact).HasMaxLength(50);
            entityTypeBuilder.Property(x => x.Email).HasMaxLength(200);
            entityTypeBuilder.Property(x => x.Password).HasMaxLength(100);
            entityTypeBuilder.Property(x => x.Gender).HasMaxLength(50);
            entityTypeBuilder.Property(x => x.DateOfBirth).HasMaxLength(100);
            entityTypeBuilder.Property(x => x.JoinIp).HasMaxLength(100);
            entityTypeBuilder.Property(x => x.UserType).HasMaxLength(50);
            entityTypeBuilder.Property(x => x.Address).HasMaxLength(200);
        }
    }
}
