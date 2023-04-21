using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TrackingModele.DAL.Entities;
using TrackingModele.DAL.Settings;

namespace TrackingModele.DAL.Seeds;

internal static partial class DataSeed
{
    private static readonly PasswordHasher<Employee> _passwordHasher = new();

    #region Roles Settings

    private static readonly Guid admin_roleId = new Guid("34cf9786-32a9-4be8-9895-18ae254c07e2");
    private static readonly Guid user_roleId = new Guid("a416d06c-9770-4dfd-8bc0-fb0245c00cdd");

    #endregion Roles Settings

    #region Users Settings

    private static readonly Guid adminId = new Guid("f97259e3-27b4-43b1-8a65-447075a108e6");
    private static readonly string admin_securityStamp = new Guid("78b49c32-2bd4-416b-be2c-cc6a45285bfb").ToString();
    private static readonly Guid user1Id = new Guid("8fc8ed1a-9a96-4710-984d-51ada27f767e");
    private static readonly string user1_securityStamp = new Guid("156bf0a8-5ed3-4e65-80c2-98f299cf70a4").ToString();


    #endregion Users Settings

    public static void AddEmployeeManagementSeed(this ModelBuilder modelBuilder, DefaultAdminSettings defaultAdminSettings)
    {
        modelBuilder.AddRoles();
        modelBuilder.AddAdmin(defaultAdminSettings);
        modelBuilder.AddUsers();
    }

    private static void AddRoles(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IdentityRole<Guid>>().HasData(
            new IdentityRole<Guid>()
            {
                Id = admin_roleId,
                Name = "Admin",
                ConcurrencyStamp = "1",
                NormalizedName = "Admin".ToUpper()
            },
            new IdentityRole<Guid>()
            {
                Id = user_roleId,
                Name = "User",
                ConcurrencyStamp = "2",
                NormalizedName = "User".ToUpper()
            });
    }

    private static void AddAdmin(this ModelBuilder modelBuilder, DefaultAdminSettings defaultAdminSettings)
    {
        Employee admin = new()
        {
            Id = adminId,
            Sex = "male",
            UserName = defaultAdminSettings.UserName,
            NormalizedUserName = defaultAdminSettings.UserName.ToUpper(),
            Email = defaultAdminSettings.Email,
            NormalizedEmail = defaultAdminSettings.Email.ToUpper(),
            DateOfBirth = defaultAdminSettings.DateOfBirth,
            EmailConfirmed = true,
            SecurityStamp = admin_securityStamp
        };

        admin.PasswordHash = _passwordHasher.HashPassword(admin, defaultAdminSettings.Password);

        modelBuilder.Entity<Employee>().HasData(admin);

        modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(
            new IdentityUserRole<Guid>()
            {
                RoleId = admin_roleId,
                UserId = adminId
            });
    }

    private static void AddUsers(this ModelBuilder modelBuilder)
    {
        #region Users

        Employee employee1 = new()
        {
            Id = user1Id,
            Sex = "male",
            UserName = "user1",
            NormalizedUserName = "user1".ToUpper(),
            Email = "user1@gmail.com",
            NormalizedEmail = "user1@gmail.com".ToUpper(),
            DateOfBirth = new DateTime(21, 3, 21),
            PhoneNumber = "0684573600",
            EmailConfirmed = true,
            SecurityStamp = user1_securityStamp
        };
        employee1.PasswordHash = _passwordHasher.HashPassword(employee1, "PassUser1");


        modelBuilder.Entity<Employee>().HasData(employee1);

        #endregion Users

        #region Role Assignment

        modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(
        new IdentityUserRole<Guid>()
        {
            RoleId = user_roleId,
            UserId = user1Id
        });

        #endregion Role Assignment
    }
}