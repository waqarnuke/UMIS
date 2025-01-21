using System;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entites;

public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public Address? Address { get; set; }
}
