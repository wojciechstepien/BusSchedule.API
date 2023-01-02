using BusSchedule.API.Entities;

namespace BusSchedule.API.Models.ForCreation
{
    public class StopOrderForCreationDto
    {
        public StopDto? Stop { get; set; }
        public int Order { get; set; }
    }
}
