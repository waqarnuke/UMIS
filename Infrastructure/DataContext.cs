using System;
using Domain.Entites;
using Infrastructure.Config;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class DataContext(DbContextOptions options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Address> Addresses {get; set;}
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(RoleConfiguration).Assembly);
    }
}
