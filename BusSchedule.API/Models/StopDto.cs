namespace BusSchedule.API.Models
{
    public class StopDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<TimeTableDto>? TimeTableList { get; set; } = null;
        public List<BusDto>? BusList { get; set; } = null;
    }
}
