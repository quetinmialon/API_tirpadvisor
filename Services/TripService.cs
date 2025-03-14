using System.Linq;
using System.Web.Http.Routing.Constraints;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using tripAdvisorAPI.DTO.Trip;
using tripAdvisorAPI.Models;

namespace tripAdvisorAPI.Services;

public class TripService(AppDbContext context, IMapper mapper, UserService userService)
{
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    private readonly UserService _userService = userService; 

    public async Task<List<TripDTORead>> GetAllTripsAsync()
    {
        var trips = await _context.Trips
             // Inclure l'utilisateur
            .Include(t => t.TripActivities)
            .ThenInclude(ta => ta.Activity)
            .Include(t => t.User) // Inclure l'utilisateur
            .ToListAsync();

        return _mapper.Map<List<TripDTORead>>(trips);
    }

    public async Task<List<TripDTORead>>GetTripsAsyncByUserId(int UserId){
        var user = await userService.GetUserById(UserId) ?? throw new ArgumentException ("user not found");
        var trips = await _context.Trips
            .Where(u => u.UserId == UserId)
            .Include(t => t.User)
            .Include (t => t.TripActivities)
            .ThenInclude(ta => ta.Activity)
            .ToListAsync();
        return _mapper.Map<List<TripDTORead>>(trips);
    }

    public async Task<TripDTORead?> GetTripByIdAsync(int id)
    {
        var trip = await _context.Trips
            .Include(t => t.User) // Inclure l'utilisateur
            .Include(t => t.TripActivities)
            .ThenInclude(ta => ta.Activity)
            .FirstOrDefaultAsync(t => t.Id == id);

        return trip == null ? null : _mapper.Map<TripDTORead>(trip);
    }

    public async Task<bool?> CreateTripAsync(TripDTOCreate tripDto)
    {
        var user = await _context.Users.FindAsync(tripDto.UserId);
        if (user == null) return null; // Vérifie si l'utilisateur existe

        var trip = _mapper.Map<Trip>(tripDto);
        trip.User = user; // Associe l'utilisateur

        // Associer les activités si des IDs sont fournis
        if (tripDto.ActivityIds != null && tripDto.ActivityIds.Any())
        {
            var activities = await _context.Activities
                .Where(a => tripDto.ActivityIds.Contains(a.Id))
                .ToListAsync();

            trip.TripActivities = activities.Select(a => new TripActivity
            {
                ActivityId = a.Id,
                Trip = trip
            }).ToList();
        }

        _context.Trips.Add(trip);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UpdateTripAsync(int id, TripDTOCreate tripDto)
    {
        var trip = await _context.Trips
            .Include(t => t.User) // Inclure l'utilisateur
            .Include(t => t.TripActivities)
            .ThenInclude(ta => ta.Activity)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (trip == null) return false; // Vérifie si le trip existe
        var user = await _context.Users.FindAsync(tripDto.UserId);
        if (user == null) return false; // Vérifie si l'utilisateur existe
        trip.User = user; // Associe l'utilisateur
        // Associer les activités si des IDs sont fournis
        if (tripDto.ActivityIds!= null && tripDto.ActivityIds.Any())
        {
            var activities = await _context.Activities
                .Where(a => tripDto.ActivityIds.Contains(a.Id))
                .ToListAsync();

            trip.TripActivities = activities.Select(a => new TripActivity
            {
                ActivityId = a.Id,
                Trip = trip
            }).ToList();
        }

        _mapper.Map(tripDto, trip); // Mise à jour des données du trip

        _context.Entry(trip).State = EntityState.Modified; // Mettre à jour le statut de l'entité à mettre à jour dans le contexte

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteTripAsync(int id)
    {
        var trip = await _context.Trips
            .Include(t => t.TripActivities) // Inclure les relations many-to-many
            .FirstOrDefaultAsync(t => t.Id == id);

        if (trip == null) return false;

        // Supprime les relations many-to-many avant de supprimer le trip
        _context.TripActivities.RemoveRange(trip.TripActivities);
        _context.Trips.Remove(trip);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<List<TripSharedDTORead>> GetSharedTripsForUser(int userId)
{
    var trips = await _context.Trips
        .Include(t => t.User)
        .Include(t => t.SharedUsers)
            .ThenInclude(su => su.User)
        .Include(t => t.TripActivities)
            .ThenInclude(ta => ta.Activity)
        .Where(t => t.SharedUsers.Any(su => su.UserId == userId))
        .ToListAsync(); // Utilisation correcte d'une requête async

    return _mapper.Map<List<TripSharedDTORead>>(trips);
}


    public async Task<bool> CreateSharedTrip(TripSharedDTO tripDto)
{
    var trip = _mapper.Map<Trip>(tripDto);
    _context.Trips.Add(trip);
    await _context.SaveChangesAsync(); // Appel asynchrone

    return true;
}


    public async Task<TripSharedDTORead> UpdateSharedTrip(int tripId, TripSharedDTO tripDto)
{
    var trip = await _context.Trips
        .Include(t => t.SharedUsers)
        .Include(t => t.TripActivities)
        .FirstOrDefaultAsync(t => t.Id == tripId); // Utiliser FirstOrDefaultAsync()

    if (trip == null)
    {
        throw new KeyNotFoundException("Trip not found");
    }

    trip.Name = tripDto.Name;
    trip.Location = tripDto.Location;
    trip.StartDate = tripDto.StartDate;
    trip.EndDate = tripDto.EndDate;
    trip.IsPublic = tripDto.IsPublic;
    trip.Budget = tripDto.Budget;
    trip.SharedUsers = tripDto.SharedWith.Select(id => new TripUsers { UserId = id, TripId = tripId }).ToList();
    trip.TripActivities = tripDto.ActivityIds?.Select(aid => new TripActivity { ActivityId = aid, TripId = tripId }).ToList() ?? new List<TripActivity>();

    await _context.SaveChangesAsync(); // Sauvegarde asynchrone
    return _mapper.Map<TripSharedDTORead>(trip);
}


    public async Task<bool> DeleteSharedTrip(int tripId)
{
    var trip = await _context.Trips.FindAsync(tripId); // Utiliser FindAsync()
    if (trip == null)
    {
        return false;
    }
    _context.Trips.Remove(trip);
    await _context.SaveChangesAsync(); // Sauvegarde asynchrone

    return true;
}

    
}
