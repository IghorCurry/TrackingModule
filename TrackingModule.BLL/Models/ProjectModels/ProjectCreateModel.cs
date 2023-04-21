namespace TrackingModule.BLL.Models.ProjectModels
{
    public record ProjectCreateModel
    {
        public string Name { get; set; }
        public DateTime DateStart { get; init; }
        public DateTime DateEnd { get; init; }
    }
}
