using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using user_city_management_service.Domain.Entities;
using user_city_management_service.Domain.Enums;

namespace user_city_management_service.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }

    // add default user name and email for testing login
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var fullAdminUser = new User
        {
            Id = Guid.NewGuid(),
            Username = "FullAdmin",
            Email = "admin@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
            Role = Role.FullAdmin,
            IsBanned = false
        };

        modelBuilder.Entity<User>().HasData(fullAdminUser);
    }

}
