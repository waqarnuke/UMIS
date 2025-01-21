using System;
using System.Reflection;
using Domain.Entites;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure;

public class DataContextSeed
{
    public static async Task SeedAsync(DataContext context, UserManager<ApplicationUser> userManager)
    {
        if (!userManager.Users.Any(x => x.UserName == "admin@test.com"))
        {
            var user = new ApplicationUser
            {
                LastName ="admin",
                FirstName="super",
                UserName = "admin@test.com",
                Email = "admin@test.com",
            };

            await userManager.CreateAsync(user, "Pa$$w0rd");
            await userManager.AddToRoleAsync(user, "Admin");

           // await context.SaveChangesAsync();
        }
    }

}
