namespace tripAdvisorAPI.Models;

public class TripActivity
{
    public int TripId { get; set; }
    public Trip Trip { get; set; } = null!;

    public int ActivityId { get; set; }
    public Activity Activity { get; set; } = null!;
}
