using AutoMapper;
using BusSchedule.API.Models.ForCreation;

namespace BusSchedule.API.Profiles
{
    public class BusProfile : Profile
    {
        public BusProfile() 
        {
            CreateMap<Entities.Bus,Models.BusDto>();
            CreateMap<BusForCreationDto, Entities.Bus>();
        }
    }
}
