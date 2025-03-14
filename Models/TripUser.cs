namespace tripAdvisorAPI.Models;

public class TripUsers {
    public int UserId { get; set; }

    public User User { get; set; } = null!;

    public int TripId { get; set; }

    public Trip Trip { get; set; } = null!;
}