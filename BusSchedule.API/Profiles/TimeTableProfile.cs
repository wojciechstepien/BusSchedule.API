using AutoMapper;
using BusSchedule.API.Entities;
using BusSchedule.API.Models;
using BusSchedule.API.Models.ForUpdate;

namespace BusSchedule.API.Profiles
{
    public class TimeTableProfile :Profile
    {
        public TimeTableProfile()
        {
            CreateMap<TimeTable, TimeTableDto>();
            CreateMap<TimeTableForUpdateDto, Entities.TimeTable>();
        }
    }
}
