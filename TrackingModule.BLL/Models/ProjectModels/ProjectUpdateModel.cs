namespace TrackingModule.BLL.Models.ProjectModels
{
    public record ProjectUpdateModel : ProjectCreateModel
    {
        public Guid Id { get; init; }
    }
}
