namespace TrackingModule.BLL.Models.EmployeeModels
{
    public record EmployeeUpdateModel : EmployeeCreateModel
    {
        public Guid Id { get; init; }

    }
}
