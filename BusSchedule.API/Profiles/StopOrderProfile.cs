using AutoMapper;
using BusSchedule.API.Models.ForCreation;
using BusSchedule.API.Models.ForUpdate;

namespace BusSchedule.API.Profiles
{
    public class StopOrderProfile : Profile
    {
        public StopOrderProfile()
        {
            CreateMap<Entities.StopOrder, Models.StopOrderDto>();
            CreateMap<StopOrderForCreationDto, Entities.StopOrder>();
            CreateMap<StopOrderForUpdateDto, Entities.StopOrder>();
        }
    }
}
