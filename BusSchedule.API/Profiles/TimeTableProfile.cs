using AutoMapper;
using BusSchedule.API.Entities;
using BusSchedule.API.Models;

namespace BusSchedule.API.Profiles
{
    public class TimeTableProfile :Profile
    {
        public TimeTableProfile()
        {
            CreateMap<TimeTable, TimeTableDto>();
        }
    }
}
