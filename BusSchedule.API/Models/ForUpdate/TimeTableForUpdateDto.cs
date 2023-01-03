namespace BusSchedule.API.Models.ForUpdate
{
    public class TimeTableForUpdateDto
    {
        public BusDto? Bus { get; set; }
        public StopDto? Stop { get; set; }
        public TimeOnly Time { get; set; }
    }
}
