using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_MVC_Core.Data;
using Ecommerce_MVC_Core.Models;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce_MVC_Core.Code
{
    public class IdentityDbInitializer
    {
        public static ApplicationDbContext _context;

        public IdentityDbInitializer(ApplicationDbContext context)
        {
            _context = context;
        }

        public static void SeedData(UserManager<ApplicationUsers> userManager,
            RoleManager<ApplicationRoles> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        public static void SeedUsers(UserManager<ApplicationUsers> userManager)
        {
            //int cityId = _context.Cities.First(x => x.Name == "Dhaka").Id;
            if (userManager.FindByNameAsync
                    ("user1").Result == null)
            {
                ApplicationUsers user = new ApplicationUsers
                {
                    UserName = "user1",
                    Email = "user1@localhost.com",
                    Name = "Nancy Davolio",
                    DateOfBirth = new DateTime(1960, 1, 1),
                    //CityId =cityId 
                };

                IdentityResult result = userManager.CreateAsync
                    (user, "12345678").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user,
                        "User").Wait();
                }
            }


            if (userManager.FindByNameAsync
                    ("admin").Result == null)
            {
                ApplicationUsers user = new ApplicationUsers
                {
                    UserName = "admin",
                    Email = "admin@localhost.com",
                    Name = "Mr Admin ALi",
                    DateOfBirth = new DateTime(1965, 1, 1),
                    //CityId = cityId
                };

                IdentityResult result = userManager.CreateAsync
                    (user, "12345678").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user,
                        "Admin").Wait();
                }
            }
        }

        public static void SeedRoles(RoleManager<ApplicationRoles> roleManager)
        {
            if (!roleManager.RoleExistsAsync
                ("User").Result)
            {
                ApplicationRoles role = new ApplicationRoles
                {
                    Name = "User",
                    Description = "Perform normal operations."
                };
                IdentityResult roleResult = roleManager.
                    CreateAsync(role).Result;
            }


            if (!roleManager.RoleExistsAsync
                ("Admin").Result)
            {
                ApplicationRoles role = new ApplicationRoles
                {
                    Name = "Admin",
                    Description = "Perform all the operations."
                };
                IdentityResult roleResult = roleManager.
                    CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync
                ("Editor").Result)
            {
                ApplicationRoles role = new ApplicationRoles
                {
                    Name = "Editor",
                    Description = "Can Edit  all the operations."
                };
                IdentityResult roleResult = roleManager.
                    CreateAsync(role).Result;
            }
        }
    }
}
