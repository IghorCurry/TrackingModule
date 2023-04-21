using Microsoft.Extensions.DependencyInjection;
using TrackingModule.BLL.Abstractions;
using TrackingModule.BLL.Managers;

public static class ProgramExtension
{
    public static void AddBllManagers(this IServiceCollection services)
    {
        services.AddScoped<IActivitiesManager, ActivitiesManager>();
        services.AddScoped<IProjectManager, ProjectManager>();
        services.AddScoped<ITrackingManager, TrackingManager>();
        services.AddScoped<IProjectManager, ProjectManager>();
        services.AddScoped<AuthManager, AuthManager>();
        services.AddScoped<IdentityManager, IdentityManager>();
    }
}
