using AutoMapper;
using BusSchedule.API.Models.ForCreation;
using BusSchedule.API.Models.ForUpdate;

namespace BusSchedule.API.Profiles
{
    public class StopProfile : Profile
    {
        public StopProfile() 
        {
            CreateMap<Entities.Stop, Models.StopDto>();
            CreateMap<StopForCreationDto, Entities.Stop>();
            CreateMap<StopForUpdateDto, Entities.Stop>();
            CreateMap<Models.StopDto, Entities.Stop>();
        }
    }
}
