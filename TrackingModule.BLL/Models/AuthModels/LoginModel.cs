namespace TrackingModule.BLL.Models.AuthModels;
public record LoginModel
{
    public string Email { get; init; }
    public string Password { get; init; }
}