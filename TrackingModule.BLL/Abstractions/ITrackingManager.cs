using TrackingModule.BLL.Models.ActivityModels;

namespace TrackingModule.BLL.Abstractions;

public interface ITrackingManager
{
    Task<IEnumerable<ActivityViewModel>> GetTimeByIdAndDate(Guid id, DateTime fromDate, DateTime toDate);
    Task<IEnumerable<ActivityViewModel>> GetTimeByIdAndWeek(Guid id, int weekNumber);
}
