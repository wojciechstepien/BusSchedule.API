using AutoMapper;
using BusSchedule.API.Models.ForCreation;

namespace BusSchedule.API.Profiles
{
    public class StopOrderProfile : Profile
    {
        public StopOrderProfile()
        {
            CreateMap<Entities.StopOrder, Models.StopOrderDto>();
            CreateMap<StopOrderForCreationDto, Entities.StopOrder>();
        }
    }
}
