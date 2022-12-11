namespace BusSchedule.API.Models
{
    public class BusDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<StopDto>? StopsRoute { get; set; } = null;
    }
}
