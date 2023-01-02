using AutoMapper;

namespace BusSchedule.API.Profiles
{
    public class StopProfile : Profile
    {
        public StopProfile() 
        {
            CreateMap<Entities.Stop, Models.StopDto>();
        }
    }
}
