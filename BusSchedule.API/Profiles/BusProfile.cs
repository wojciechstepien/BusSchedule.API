using AutoMapper;
using BusSchedule.API.Models;
using BusSchedule.API.Models.ForCreation;
using BusSchedule.API.Models.ForUpdate;

namespace BusSchedule.API.Profiles
{
    public class BusProfile : Profile
    {
        public BusProfile() 
        {
            CreateMap<Entities.Bus,Models.BusDto>();
            CreateMap<BusForCreationDto, Entities.Bus>();
            CreateMap<BusForUpdateDto, Entities.Bus>();
            CreateMap<BusDto, Entities.Bus>();
        }
    }
}
