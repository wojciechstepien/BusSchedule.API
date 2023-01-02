using AutoMapper;
using BusSchedule.API.Models.ForCreation;

namespace BusSchedule.API.Profiles
{
    public class RouteProfile : Profile
    {
        public RouteProfile() 
        {
            CreateMap<Entities.Route, Models.RouteDto>();
            CreateMap<RouteForCreationDto, Entities.Route>();
        }
    }
}
