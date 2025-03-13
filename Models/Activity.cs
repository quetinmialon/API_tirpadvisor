using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tripAdvisorAPI.Models
{
    public class Activity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public double? Cost { get; set; }

        public string? Location { get; set; }

        // Clé étrangère vers Trip
        public ICollection<TripActivity> TripActivities { get; set; } = new List<TripActivity>();
    }
}