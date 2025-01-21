using System;
using System.Security.Claims;
using Application.DTO;
using Application.Extensions;
using Domain.Entites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController(SignInManager<ApplicationUser> signInManager ) : BaseController
{
    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterDto registerDto)
    {
        var user = new ApplicationUser
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Email = registerDto.Email,
            UserName = registerDto.Email
        };

        var result  =  await signInManager.UserManager.CreateAsync(user,registerDto.Password);
        //await signInManager.UserManager.AddToRoleAsync(user,"user")

        if(!result.Succeeded)
        {
            foreach(var error in result.Errors)
            {
                ModelState.AddModelError(error.Code,error.Description);
            }

            return ValidationProblem();
        }
        
        return Ok();
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<ActionResult> Logout()
    {
        await signInManager.SignOutAsync();

        return NoContent();
    }


    [HttpGet("user-info")]
    public async Task<ActionResult> GetUserInfo()
    {
        if(User.Identity?.IsAuthenticated == false) return NoContent();

        var user = await signInManager.UserManager.GetUserByEmailWithAddress(User);

        if(user == null) return Unauthorized();

        return Ok(new 
        {
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            Address = user.Address?.ToDto(),
            Roles = User.FindFirstValue(ClaimTypes.Role)
        });
    }

    [HttpGet("auth-status")] 
    public ActionResult GetAuthState()
    {
        return Ok(new {isAuthenticated = User.Identity?.IsAuthenticated ?? false} );
    }

    [Authorize]
    [HttpPost("address")]
    public async Task<ActionResult<Address>> CreateOrUpdateAddress(AddressDto addressDto)
    {
        var user =  await signInManager.UserManager.GetUserByEmailWithAddress (User);

        if(user.Address == null)
        {
            user.Address = addressDto.ToEntity();
        }
        else
        {
            user.Address.UpdateFromDto(addressDto);
        }

        var result = await signInManager.UserManager.UpdateAsync(user);

        if(!result.Succeeded) return BadRequest("Problem update user address");

        return Ok(user.Address.ToDto());
    }

}
