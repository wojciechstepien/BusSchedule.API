namespace BusSchedule.API.Models
{
    public class RouteDto
    {
        public int Id { get; set; }
        public BusDto? Bus { get; set; }
        public List<StopOrderDto> StopsOrders { get; set; } = new List<StopOrderDto>();
    }
}
