namespace BusSchedule.API.Models.ForCreation
{
    public class TimeTableForCreationDto
    {
        public BusDto? Bus { get; set; }
        public StopDto? Stop { get; set; }
        public TimeOnly Time { get; set; }
    }
}
