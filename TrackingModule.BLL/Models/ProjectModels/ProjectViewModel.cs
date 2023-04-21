namespace TrackingModule.BLL.Models.ProjectModels
{
    public record ProjectViewModel
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public DateTime DateStart { get; init; }
        public DateTime DateEnd { get; init; }
    }
}
