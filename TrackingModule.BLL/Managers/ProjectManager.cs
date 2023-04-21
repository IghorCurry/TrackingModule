using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TrackingModele.DAL;
using TrackingModele.DAL.Entities;
using TrackingModule.BLL.Abstractions;
using TrackingModule.BLL.Models.ActivityModels;
using TrackingModule.BLL.Models.ProjectModels;

namespace TrackingModule.BLL.Managers
{
    public class ProjectManager : IProjectManager
    {
        private readonly DataContext _dataContext;
        private readonly ILogger _logger;
        public ProjectManager(DataContext context, ILogger<ProjectManager> logger)
        {
            _dataContext = context;
            _logger = logger;
        }
        public async Task<ProjectViewModel> CreateProjectAsync(ProjectCreateModel model)
        {

            try
            {
                var project = model.Adapt<Project>();

                _dataContext.Projects.Add(project);
                _logger.LogInformation("Project created.");
                await _dataContext.SaveChangesAsync();
                return project.Adapt<ProjectViewModel>();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while creating a project");
            };
        }

        public async Task<bool> DeleteProjectAsync(Guid id)
        {
            var project = new Project { Id = id };

            try
            {
                _dataContext.Entry(project).State = EntityState.Deleted;
                _logger.LogInformation("Project deleted.");
                await _dataContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while deleting a project");
            }
        }

        public async Task<IEnumerable<ProjectViewModel>> GetProjectsAsync()
        {
            try
            {
                return await _dataContext.Projects
                    .ProjectToType<ProjectViewModel>()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while getting a projects");
            }
        }

        public async Task<ProjectViewModel> GetProjectAsync(Guid id)
        {
            try
            {
                return await _dataContext.Projects.ProjectToType<ProjectViewModel>().FirstOrDefaultAsync(x => x.Id == id);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while getting a project");
            }
        }

        public async Task<bool> IsExists(Guid id)
        {
            return await _dataContext.Projects.AnyAsync(c => c.Id == id);
        }

        public async Task<ProjectViewModel> UpdateProjectAsync(ProjectUpdateModel model)
        {
            try
            {
                var project = model.Adapt<Project>();

                _dataContext.Entry(project).State = EntityState.Modified;
                _logger.LogInformation("Project updated.");

                await _dataContext.SaveChangesAsync();
                return project.Adapt<ProjectViewModel>();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while updating a project");
            }
        }
    }
}
