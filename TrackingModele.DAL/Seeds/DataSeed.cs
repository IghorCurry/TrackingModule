using Microsoft.EntityFrameworkCore;
using TrackingModele.DAL.Settings;

namespace TrackingModele.DAL.Seeds;

internal static partial class DataSeed
{
    public static void AddTestableData(this ModelBuilder modelBuilder, DefaultAdminSettings defaultAdminSettings)
    {
        modelBuilder.AddEmployeeManagementSeed(defaultAdminSettings);
    }
}