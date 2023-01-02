using AutoMapper;
using BusSchedule.API.Models.ForCreation;

namespace BusSchedule.API.Profiles
{
    public class StopProfile : Profile
    {
        public StopProfile() 
        {
            CreateMap<Entities.Stop, Models.StopDto>();
            CreateMap<StopForCreationDto, Entities.Stop>();
        }
    }
}
