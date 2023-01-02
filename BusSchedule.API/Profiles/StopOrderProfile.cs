using AutoMapper;

namespace BusSchedule.API.Profiles
{
    public class StopOrderProfile : Profile
    {
        public StopOrderProfile()
        {
            CreateMap<Entities.StopOrder, Models.StopOrderDto>();
        }
    }
}
