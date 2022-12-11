namespace BusSchedule.API.Models
{
    public class RouteDto
    {
        public BusDto? Bus { get; set; } = null;
        public List<StopDto>? StopList { get; set; } = null;

    }
}
