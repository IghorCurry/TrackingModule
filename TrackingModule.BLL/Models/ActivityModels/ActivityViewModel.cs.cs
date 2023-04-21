using TrackingModele.DAL.Entities;
using TrackingModule.BLL.Models.EmployeeModels;
using TrackingModule.BLL.Models.ProjectModels;

namespace TrackingModule.BLL.Models.ActivityModels
{
    public record ActivityViewModel : ActivityUpdateModel
    {
        public EmployeeViewModel Employee { get; init; }
        public ProjectViewModel Project { get; init; }
        public ActivityType ActivityType { get; init; }
        public DateTime FromDate { get; init; }
        public DateTime ToDate { get; init; }
    }
}
