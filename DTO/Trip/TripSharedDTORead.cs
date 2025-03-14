using tripAdvisorAPI.DTO;
using tripAdvisorAPI.Models;
namespace tripAdvisorAPI.DTO.Trip;
public class TripSharedDTORead
{
    public int Id { get; set; }
    public string Location { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsPublic { get; set; }
    public double? Budget { get; set; }
    public string Name { get; set; }

    public string AuthorName { get; set; }

    public List<string> SharedWith { get; set; } = new List<string>();

    public List<string>? ActivityNames { get; set; }

}