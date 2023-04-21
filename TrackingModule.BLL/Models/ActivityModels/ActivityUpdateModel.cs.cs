namespace TrackingModule.BLL.Models.ActivityModels
{
    public record ActivityUpdateModel : ActivityCreateModel
    {
        public Guid Id { get; init; }
    }
}
