using AutoMapper;
using Microsoft.EntityFrameworkCore;
using tripAdvisorAPI.DTO;
using tripAdvisorAPI.Models;

namespace tripAdvisorAPI.Services;

public class ActivityService(AppDbContext context, IMapper mapper)
{
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    // 🚀 Récupérer toutes les activités
    public async Task<IEnumerable<ActivityDTORead>> GetAllActivitiesAsync()
    {
        var activities = await _context.Activities
            .Include(a => a.TripActivities)
            .ThenInclude(ta => ta.Trip)
            .ToListAsync();

        return _mapper.Map<IEnumerable<ActivityDTORead>>(activities);
    }

    // 🚀 Récupérer une activité par ID
    public async Task<ActivityDTORead?> GetActivityByIdAsync(int id)
    {
        var activity = await _context.Activities
            .Include(a => a.TripActivities)
            .ThenInclude(ta => ta.Trip)
            .FirstOrDefaultAsync(a => a.Id == id);

        return activity == null ? null : _mapper.Map<ActivityDTORead>(activity);
    }

    // 🚀 Créer une activité et l'associer à des voyages
    public async Task<bool> CreateActivityAsync(ActivityDTO activityDto)
    {
        var activity = _mapper.Map<Activity>(activityDto);
        _context.Activities.Add(activity);
        await _context.SaveChangesAsync();
        return true;
    }

    // 🚀 Mettre à jour une activité
    public async Task<bool> UpdateActivityAsync(int id, ActivityDTO activityDto)
    {
        var activity = await _context.Activities
            .Include(a => a.TripActivities)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (activity == null) return false;

        // Mise à jour des données de l'activité
        _mapper.Map(activityDto, activity);

        // Mise à jour des relations many-to-many
        activity.TripActivities = activityDto.TripIds != null
            ? activityDto.TripIds.Select(tid => new TripActivity { TripId = tid, ActivityId = id }).ToList()
            : new List<TripActivity>();

        await _context.SaveChangesAsync();
        return true;
    }

    // 🚀 Supprimer une activité
    public async Task<bool> DeleteActivityAsync(int id)
    {
        var activity = await _context.Activities.FindAsync(id);
        if (activity == null) return false;

        _context.Activities.Remove(activity);
        await _context.SaveChangesAsync();
        return true;
    }
}
