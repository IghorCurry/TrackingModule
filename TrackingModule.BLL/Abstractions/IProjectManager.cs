using TrackingModule.BLL.Models.ProjectModels;

namespace TrackingModule.BLL.Abstractions
{
    public interface IProjectManager
    {
        Task<IEnumerable<ProjectViewModel>> GetProjectsAsync();
        Task<ProjectViewModel> GetProjectAsync(Guid id);
        Task<ProjectViewModel> CreateProjectAsync(ProjectCreateModel model);
        Task<bool> IsExists(Guid id);
        Task<ProjectViewModel> UpdateProjectAsync(ProjectUpdateModel model);
        Task<bool> DeleteProjectAsync(Guid id);
    }
}
