using TrackingModele.DAL.Entities;

namespace TrackingModule.BLL.Models.ActivityModels
{
    public record ActivityCreateModel
    {
        //public int EmployeeId { get; init; }
        public Guid ProjectId { get; init; }
        public Guid EmployeeId { get; init; }
        public ActivityType ActivityType { get; init; }
        public DateTime FromDate { get; init; }
        public DateTime ToDate { get; init; }

    }
}
