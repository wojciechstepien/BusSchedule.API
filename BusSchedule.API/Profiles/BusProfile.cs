using AutoMapper;

namespace BusSchedule.API.Profiles
{
    public class BusProfile : Profile
    {
        public BusProfile() 
        {
            CreateMap<Entities.Bus,Models.BusDto>();
        }
    }
}
