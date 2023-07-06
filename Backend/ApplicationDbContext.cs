using Backend.Model.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Backend;

public class ApplicationDbContext : IdentityDbContext<Employee>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Banner> Banners { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Employee> Employees { get; set; } = null!;
    public DbSet<Facility> Facilities { get; set; } = null!;
    public DbSet<Feedback> Feedbacks { get; set; } = null!;
    public DbSet<Store> Stores { get; set; } = null!;
    public DbSet<StoreBanner> StoreBanners { get; set; } = null!;
    public DbSet<StoreItem> StoreItems { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            var tableName = entityType.GetTableName();
            if (tableName != null && tableName.StartsWith("AspNet"))
            {
                entityType.SetTableName(tableName[6..]);
            }
        }
    }
}