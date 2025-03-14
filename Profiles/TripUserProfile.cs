using AutoMapper;
using tripAdvisorAPI.DTO.Trip;
using tripAdvisorAPI.Models;

namespace tripAdvisorAPI.Profiles;

public class TripUserProfile : Profile
{
    public TripUserProfile()
    {
        // Mapping de Trip vers TripSharedDTORead
        CreateMap<Trip, TripSharedDTORead>()
            .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.User.FirstName))
            .ForMember(dest => dest.SharedWith, opt => opt.MapFrom(src => src.SharedUsers.Select(su => su.User.FirstName).ToList()))
            .ForMember(dest => dest.ActivityNames, opt => opt.MapFrom(src => src.TripActivities.Select(ta => ta.Activity.Name).ToList()));

        // Mapping de TripSharedDTO vers Trip
        CreateMap<TripSharedDTO, Trip>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.SharedUsers, opt => opt.MapFrom(src => src.SharedWith.Select(id => new TripUsers { UserId = id }).ToList()))
            .ForMember(dest => dest.TripActivities, opt => opt.MapFrom(src => src.ActivityIds.Select(aid => new TripActivity { ActivityId = aid }).ToList()));
    }
}
