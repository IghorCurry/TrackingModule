namespace TrackingModele.DAL.Entities;

public class Activity
{
    public Guid Id { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; }
    public Guid ProjectId { get; set; }
    public Project Project { get; set; }
    public ActivityType ActivityType { get; set; }

}
