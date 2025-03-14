using tripAdvisorAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace tripAdvisorAPI;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Trip> Trips { get; set; }
    public DbSet<Activity> Activities { get; set; }
    public DbSet<TripActivity> TripActivities { get; set; } // Ajout de l'entité pivot

    public DbSet<TripUsers> TripUsers { get; set; } // Ajout de l'entité pivot pour les voyages partagés

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 🚀 Relation One-to-Many : User → Trips
        modelBuilder.Entity<User>()
            .HasMany(u => u.Trips)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Restrict); // ⚠️ Empêche la suppression cascade sur User

        // 🚀 Relation Many-to-Many : Trip ↔ Activity via TripActivity
        modelBuilder.Entity<TripActivity>()
            .HasKey(ta => new { ta.TripId, ta.ActivityId }); // Clé composite

        modelBuilder.Entity<TripActivity>()
            .HasOne(ta => ta.Trip)
            .WithMany(t => t.TripActivities)
            .HasForeignKey(ta => ta.TripId)
            .OnDelete(DeleteBehavior.Cascade); // ⚠️ Si un Trip est supprimé, ses relations le sont aussi

        modelBuilder.Entity<TripActivity>()
            .HasOne(ta => ta.Activity)
            .WithMany(a => a.TripActivities)
            .HasForeignKey(ta => ta.ActivityId)
            .OnDelete(DeleteBehavior.Cascade); // ⚠️ Si une Activity est supprimée, ses relations aussi

        // Relation Many-to-Many : Trip // User pour les voyages partagés
        modelBuilder.Entity<TripUsers>()
           .HasKey(tu => new { tu.TripId, tu.UserId });

        modelBuilder.Entity<TripUsers>()
           .HasOne(tu => tu.Trip)
           .WithMany(t => t.SharedUsers)
           .HasForeignKey(tu => tu.TripId)
           .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TripUsers>()
           .HasOne(tu => tu.User)
           .WithMany(u => u.SharedTrips)
           .HasForeignKey(tu => tu.UserId)
           .OnDelete(DeleteBehavior.Restrict); // ⚠️ Empêche la suppression cascade sur User
    }
}
