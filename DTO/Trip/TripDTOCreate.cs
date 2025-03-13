namespace tripAdvisorAPI.DTO.Trip;

public class TripDTOCreate
{ 
    public int Id { get; set; }
    public string Location { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool isPublic { get; set; }
    public int? Budget { get; set; }
    public string Name { get; set; }
    public int UserId { get; set; }
    public List<int>? ActivityIds { get; set; }
}