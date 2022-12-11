namespace BusSchedule.API.Models
{
    public class TimeTableDto
    {
        public BusDto? Bus { get; set; } = null;
        public StopDto? Stop { get; set; } = null;
        public List<TimeOnly>? Time { get; set; } = null;
    }
}
