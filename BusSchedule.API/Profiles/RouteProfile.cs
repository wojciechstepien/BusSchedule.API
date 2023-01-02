using AutoMapper;

namespace BusSchedule.API.Profiles
{
    public class RouteProfile : Profile
    {
        public RouteProfile() 
        {
            CreateMap<Entities.Route, Models.RouteDto>();
        }
    }
}
