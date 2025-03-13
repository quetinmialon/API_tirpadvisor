using AutoMapper;
using tripAdvisorAPI.DTO.Trip;
using tripAdvisorAPI.Models;

namespace tripAdvisorAPI.Profiles;

public class TripProfile : Profile
{
    public TripProfile()
    {
        // Mapping Trip → TripDTORead (Inclut User et Activités)
        CreateMap<Trip, TripDTORead>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName)) // Afficher le prénom de l'utilisateur
            .ForMember(dest => dest.ActivityNames, 
                opt => opt.MapFrom(src => src.TripActivities.Select(ta => ta.Activity.Name)));


        // Mapping TripDTOCreate → Trip (Sans recréer un User)
        CreateMap<TripDTOCreate, Trip>()
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.TripActivities, 
                opt => opt.Ignore());
            
    }
}
