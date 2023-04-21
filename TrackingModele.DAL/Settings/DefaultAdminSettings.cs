namespace TrackingModele.DAL.Settings;
public record DefaultAdminSettings
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Password { get; set; }
}