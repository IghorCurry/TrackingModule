using TrackingModule.BLL.Models.ActivityModels;

namespace TrackingModule.BLL.Abstractions
{
    public interface IActivitiesManager
    {
        Task<IEnumerable<ActivityViewModel>> GetActivitiesAsync();
        Task<ActivityViewModel> GetActivityAsync(Guid id);

        Task<ActivityViewModel> CreateActivityAsync(ActivityCreateModel model);
        Task<bool> IsExists(Guid id);
        Task<ActivityViewModel> UpdateActivityAsync(ActivityUpdateModel model);
        Task<bool> DeleteActivityAsync(Guid id);

    }
}
