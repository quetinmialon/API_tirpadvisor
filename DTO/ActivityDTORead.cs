namespace tripAdvisorAPI.DTO;

public class ActivityDTORead
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public double? Cost { get; set; }
    public string? Location { get; set; }
    public DateTime Date { get; set; }

    public List<string>? TripNames { get; set; } // Liste des noms des voyages liés
}
