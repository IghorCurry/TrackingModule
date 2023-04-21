namespace TrackingModule.BLL.Models.EmployeeModels
{
    public record EmployeeViewModel
    {
        public Guid Id { get; init; }
        public string UserName { get; init; }

        public IList<string> RoleNames { get; set; }
    }
}
