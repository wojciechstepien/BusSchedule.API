using BusSchedule.API.Entities;

namespace BusSchedule.API.Models
{
    public class StopOrderDto
    {
        public int Id { get; set; }
        public Stop? Stop { get; set; }
        public int Order { get; set; }
    }
}
