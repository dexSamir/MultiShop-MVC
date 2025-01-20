using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MultiShop.Core.Entities;

namespace MultiShop.DAL.Context; 
public class AppDbContext : IdentityDbContext<User>
{
	public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories{ get; set; }

    public AppDbContext(DbContextOptions opt) : base(opt)
	{
	}

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly); 
        base.OnModelCreating(builder);
    }
}

