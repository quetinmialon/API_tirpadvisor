using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tripAdvisorAPI.Models
{
    public class Trip
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [MaxLength(255)]
        public string Location { get; set; }

        public double? Budget { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public bool IsPublic { get; set; } = false;

        // Clé étrangère vers User
        [ForeignKey("User")]
        public int UserId { get; set; }

        // Navigation vers User
        public User User { get; set; }

        // Relation Many-to-Many avec Activity
        public ICollection<TripActivity> TripActivities { get; set; } = new List<TripActivity>();
    }
}