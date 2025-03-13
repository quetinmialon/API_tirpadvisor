namespace tripAdvisorAPI.DTO;

public class ActivityDTO
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public double? Cost { get; set; }
    public string? Location { get; set; }
    public DateTime Date { get; set; }

    public List<int>? TripIds { get; set; } // Liste des IDs des trips associés
}
