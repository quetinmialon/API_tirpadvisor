using System.ComponentModel.DataAnnotations;

namespace tripAdvisorAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string? FirebaseUid { get; set; }

        // Relation One-to-Many avec Trip
        public List<Trip> Trips { get; set; } = new List<Trip>();

        //relation many to many avec Trips pour les voyages partagés
        public List<TripUsers> SharedTrips { get; set; } = new List<TripUsers>();
    }
}