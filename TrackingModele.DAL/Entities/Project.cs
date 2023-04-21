namespace TrackingModele.DAL.Entities;

public class Project
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime DateStart { get; set; }
    public DateTime DateEnd { get; set; }
}
