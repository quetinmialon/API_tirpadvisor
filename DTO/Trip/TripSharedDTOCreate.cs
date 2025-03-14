using tripAdvisorAPI.DTO;
using tripAdvisorAPI.Models;
namespace tripAdvisorAPI.DTO.Trip;
public class TripSharedDTO
{
    public int Id { get; set; }
    public string Location { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsPublic { get; set; }
    public double? Budget { get; set; }
    public string Name { get; set; }

    public int UserId { get; set; }

    public List<int> SharedWith { get; set; } = new List<int>();

    public List<int>? ActivityIds { get; set; }

}