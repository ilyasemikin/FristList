using FristList.Service.Data.Models.Account;
using FristList.Service.Data.Models.Activities;
using FristList.Service.Data.Models.Categories;
using FristList.Service.Data.Models.Categories.Base;
using Microsoft.EntityFrameworkCore;

namespace FristList.Service.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

    public DbSet<BaseCategory> Categories { get; set; } = null!;
    public DbSet<PersonalCategory> PersonalCategories { get; set; } = null!;

    public DbSet<Activity> Activities { get; set; } = null!;
    public DbSet<CurrentActivity> CurrentActivities { get; set; } = null!;
    
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BaseCategory>()
            .ToTable("Categories");
        modelBuilder.Entity<ActivityCategory>()
            .ToTable("ActivityCategories");
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}