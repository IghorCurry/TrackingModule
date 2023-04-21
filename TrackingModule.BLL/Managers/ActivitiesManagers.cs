using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TrackingModele.DAL;
using TrackingModele.DAL.Entities;
using TrackingModule.BLL.Abstractions;
using TrackingModule.BLL.Models.ActivityModels;

namespace TrackingModule.BLL.Managers
{
    public class ActivitiesManager : IActivitiesManager
    {
        private readonly DataContext _dataContext;
        private readonly ILogger _logger;
        public ActivitiesManager(DataContext context, ILogger<ActivitiesManager> logger)
        {
            _dataContext = context;
            _logger = logger;
        }
        public async Task<ActivityViewModel> CreateActivityAsync(ActivityCreateModel model)
        {
           
                var activity = model.Adapt<Activity>();

                _dataContext.Activities.Add(activity);
                _logger.LogInformation("Activity created.");
                await _dataContext.SaveChangesAsync();
                return activity.Adapt<ActivityViewModel>();
            
        }

        public async Task<bool> DeleteActivityAsync(Guid id)
        {
            var activity = new Activity { Id = id };

            try
            {
                _dataContext.Entry(activity).State = EntityState.Deleted;
                _logger.LogInformation("Activity deleted.");
                await _dataContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while getting a models");
            }
        }

        public async Task<IEnumerable<ActivityViewModel>> GetActivitiesAsync()
        {
            
                return await _dataContext.Activities
            .Include(a => a.Employee)
            .Include(a => a.Project)
            .ProjectToType<ActivityViewModel>()
            .ToListAsync();
           

        }

        public async Task<ActivityViewModel> GetActivityAsync(Guid id)
        {
            try
            {
                var activity = await _dataContext.Activities
                .Include(a => a.Employee)
                .Include(a => a.Project)
                .ProjectToType<ActivityViewModel>()
                .FirstOrDefaultAsync(a => a.Id == id);

                return activity;
            }
            catch
            {
                throw new Exception("An error occured while getting a model");
            }
        }
        public async Task<bool> IsExists(Guid id)
        {
            return await _dataContext.Activities.AnyAsync(c => c.Id == id);
        }

        public async Task<ActivityViewModel> UpdateActivityAsync(ActivityUpdateModel model)
        {
            try
            {
                var activity = model.Adapt<Activity>();

                _dataContext.Entry(activity).State = EntityState.Modified;
                _logger.LogInformation("Activity updated.");

                await _dataContext.SaveChangesAsync();
                return activity.Adapt<ActivityViewModel>();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while updating a model");
            }
        }
    }
}


