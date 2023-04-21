using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TrackingModele.DAL;
using TrackingModule.BLL.Abstractions;
using TrackingModule.BLL.Models.ActivityModels;

namespace TrackingModule.BLL.Managers;

public class TrackingManager : ITrackingManager
{
    private readonly DataContext _dataContext;
    private readonly ILogger _logger;
    public TrackingManager(DataContext context, ILogger<TrackingManager> logger)
    {
        _dataContext = context;
        _logger = logger;
    }
    public async Task<IEnumerable<ActivityViewModel>> GetTimeByIdAndDate(Guid id, DateTime fromDate, DateTime toDate)
    {
        try
        {
            var activities = await _dataContext.Activities
                .Include(a => a.Project)
                .Where(a => a.EmployeeId == id && (a.FromDate >= fromDate && a.FromDate <= toDate))
                .Select(a => a.Adapt<ActivityViewModel>())
                .ToListAsync();

            return activities;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while getting time tracking for employee.");
            return Enumerable.Empty<ActivityViewModel>();
        }
    }

    public async Task<IEnumerable<ActivityViewModel>> GetTimeByIdAndWeek(Guid id, int weekNumber)
    {
        try
        {
            var startDate = new DateTime(DateTime.Now.Year, 1, 1);
            var startOfWeek = startDate.AddDays((weekNumber - 1) * 7).AddDays(-(int)startDate.DayOfWeek);
            var endOfWeek = startOfWeek.AddDays(6);

            var timeTrackings = await _dataContext.Activities
                .Include(tt => tt.Employee)
                .Include(tt => tt.Project)
                .Where(tt => tt.Employee.Id == id && tt.FromDate >= startOfWeek && tt.ToDate <= endOfWeek)
                .ProjectToType<ActivityViewModel>()
                .ToListAsync();


            return timeTrackings;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while getting time tracking for employee.");
            return Enumerable.Empty<ActivityViewModel>();
        }
    }

}
