namespace TrackingModule.BLL.Models.EmployeeModels;

public record EmployeeCreateModel
{
    public string UserName { get; init; }
    public string Sex { get; init; }
    public DateTime Birthday { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
}
