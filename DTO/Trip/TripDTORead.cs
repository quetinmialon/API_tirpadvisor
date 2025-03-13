using tripAdvisorAPI.DTO;
namespace tripAdvisorAPI.DTO.Trip;
public class TripDTORead
{
    public int Id { get; set; }
    public required string Location { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool isPublic { get; set; }
    public int? Budget { get; set; }
    public required string Name { get; set; }
    public required string FirstName { get; set; }
    public List<string>? ActivityNames { get; set; }

}
