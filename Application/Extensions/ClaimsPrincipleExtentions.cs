using System.Security.Authentication;
using System.Security.Claims;
using Domain.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Extensions;

public static class ClaimsPrincipleExtentions
{

    public static async Task<ApplicationUser> GetUserByEmail(this UserManager<ApplicationUser> userManager, 
        ClaimsPrincipal user)
    {
        var userToReturn = await userManager.Users.FirstOrDefaultAsync(x =>x.Email == user.GetEmail());

        if(userToReturn == null) throw new AuthenticationException("User not found");

        return userToReturn;
    }
    public static string GetEmail(this ClaimsPrincipal user)
    {
        var email = user.FindFirstValue(ClaimTypes.Email) ?? throw new AuthenticationException("Email claim not found");

        return email;
    }

    public static async Task<ApplicationUser> GetUserByEmailWithAddress(this UserManager<ApplicationUser> userManager, 
        ClaimsPrincipal user)
    {
        var userToReturn = await userManager.Users
            .Include(x => x.Address)
            .FirstOrDefaultAsync(x =>x.Email == user.GetEmail());

        if(userToReturn == null) throw new AuthenticationException("User not found");

        return userToReturn;
    }

}
