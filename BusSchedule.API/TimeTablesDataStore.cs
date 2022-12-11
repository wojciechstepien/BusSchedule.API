using BusSchedule.API.Models;

namespace BusSchedule.API
{
    public class TimeTablesDataStore
    {
        public static TimeTablesDataStore Instance { get; private set; } = new TimeTablesDataStore();
        public List<TimeTableDto> TimeTables { get; set; }

        public TimeTablesDataStore()
        {
            TimeTables = new List<TimeTableDto>()
            { 
                new TimeTableDto
                {
                    Bus = BusesDataStore.Instance.Buses.ToArray()[0],
                    Stop = StopsDataStore.Instance.Stops.ToArray()[0],
                    Times = new List<TimeOnly>
                    { new TimeOnly(1,0), new TimeOnly(2,0), new TimeOnly(3,0) }
                },
                new TimeTableDto
                {
                    Bus = BusesDataStore.Instance.Buses.ToArray()[0],
                    Stop = StopsDataStore.Instance.Stops.ToArray()[1],
                    Times = new List<TimeOnly>
                    { new TimeOnly(1,10), new TimeOnly(2,10), new TimeOnly(3,10) }
                },
                new TimeTableDto
                {
                    Bus = BusesDataStore.Instance.Buses.ToArray()[0],
                    Stop = StopsDataStore.Instance.Stops.ToArray()[2],
                    Times = new List<TimeOnly>
                    { new TimeOnly(1,20), new TimeOnly(2,20), new TimeOnly(3,20) }
                }
            };
        }
    }
}
