using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System;
using EShopping.Models.Enums;


namespace EShopping.Data
{
    // Singleton
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedIdentityRolesAsync(RoleManager<IdentityRole> rolemanager)
        {
            foreach (MyIdentityRoleNames rolename in Enum.GetValues(typeof(MyIdentityRoleNames)))
            {
                if (!await rolemanager.RoleExistsAsync(rolename.ToString()))
                {
                    await rolemanager.CreateAsync(
                        new IdentityRole { Name = rolename.ToString() });
                }
            }
        }

        public static async Task SeedIdentityUserAsync(UserManager<IdentityUser> usermanager)
        {
            IdentityUser user;

            user = await usermanager.FindByNameAsync("admin@demo.com");
            if (user == null)
            {
                user = new IdentityUser()
                {
                    UserName = "admin@demo.com",
                    Email = "admin@demo.com",
                    EmailConfirmed = true
                };
                await usermanager.CreateAsync(user, password: "Password@123");

                await usermanager.AddToRolesAsync(user, new string[] {
                    MyIdentityRoleNames.ProductAdmin.ToString(),
                    MyIdentityRoleNames.ProductMember.ToString()
                });
            }

            user = await usermanager.FindByNameAsync("user@demo.com");
            if (user == null)
            {
                user = new IdentityUser()
                {
                    UserName = "user@demo.com",
                    Email = "user@demo.com",
                    EmailConfirmed = true
                };
                await usermanager.CreateAsync(user, password: "Asdf@123");
                //await usermanager.AddToRolesAsync(user, new string[] {
                //    MyIdentityRoleNames.AppUser.ToString()
                //});
                await usermanager.AddToRoleAsync(user, MyIdentityRoleNames.ProductMember.ToString());
            }
        }

    }
}



