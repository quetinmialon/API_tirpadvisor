using AutoMapper;
using tripAdvisorAPI.Models;
using tripAdvisorAPI.DTO;

namespace tripAdvisorAPI.Profiles;

public class ActivityProfile : Profile
{
    public ActivityProfile()
    {
        // 🚀 Mapping pour la lecture des activités
        CreateMap<Activity, ActivityDTORead>()
            .ForMember(dest => dest.TripNames, 
                       opt => opt.MapFrom(src => src.TripActivities.Select(ta => ta.Trip.Name).ToList()));

        // 🚀 Mapping pour la création d'activités
        CreateMap<ActivityDTO, Activity>()
            .ForMember(dest => dest.TripActivities, 
                       opt => opt.MapFrom(src => src.TripIds != null 
                            ? src.TripIds.Select(id => new TripActivity { TripId = id }).ToList() 
                            : new List<TripActivity>()));
    }
}
