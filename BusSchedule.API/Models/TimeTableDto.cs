namespace BusSchedule.API.Models
{
    public class TimeTableDto
    {
        public BusDto? Bus { get; set; } = null;
        public StopDto? Stop { get; set; } = null;
        public List<TimeOnly>? Times { get; set; } = null;
    }
}
