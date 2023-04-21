using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TrackingModele.DAL.Configuration;
using TrackingModele.DAL.Entities;
using TrackingModele.DAL.Seeds;
using TrackingModele.DAL.Settings;
using Microsoft.Extensions.Options;

namespace TrackingModele.DAL;

public class DataContext : IdentityDbContext<Employee, IdentityRole<Guid>, Guid>
{
    private DefaultAdminSettings _defaultAdminSettings { get; init; }
    public DataContext(DbContextOptions<DataContext> options, IOptions<DefaultAdminSettings> defaultAdminSettings)
            : base(options) => _defaultAdminSettings = defaultAdminSettings.Value;

    public DbSet<Employee> Employees { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Activity> Activities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.AddTestableData(_defaultAdminSettings);
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new ActivityConfiguration());

    }
}
