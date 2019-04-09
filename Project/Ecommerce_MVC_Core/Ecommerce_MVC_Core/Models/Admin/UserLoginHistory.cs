using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_MVC_Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce_MVC_Core.Models.Admin
{
    public class UserLoginHistory:BaseEntity
    {
       

        public string UserId { get; set; }
        public string IpAddress { get; set; }

        public  ApplicationUsers Users { get; set; }
    }
    /*
    public class UserLoginHistoryMap
    {
        public UserLoginHistoryMap(EntityTypeBuilder<UserLoginHistory> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.IpAddress).HasMaxLength(50);
            entityTypeBuilder.Property(x => x.AddedDate).HasDefaultValue(DateTime.Now);
            entityTypeBuilder.HasOne(x => x.Users).WithMany(x => x.UserLoginHistories).HasForeignKey(x => x.UserId);
        }
    }
    */
}
