using Microsoft.AspNetCore.Identity;

namespace TrackingModele.DAL.Entities;

public class Employee : IdentityUser<Guid>
{
    public Guid Id { get; set; }
    public string Sex { get; set; }
    public DateTime DateOfBirth { get; set; }
}
