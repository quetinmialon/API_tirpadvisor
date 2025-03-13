namespace tripAdvisorAPI.DTO;
using tripAdvisorAPI.DTO.Trip;

public class UserDTO
{
    public int Id { get; set; }
    
    public string FirstName { get; set; }
    
    public List<TripDTORead>? Trips { get; set; } = new();
    
}