using BusSchedule.API.Validation;

namespace BusSchedule.API.Models.ForUpdate
{
    public class BusForUpdateDto
    {
        public string Name { get; set; } = string.Empty;
        [StopsInListExists(ErrorMessage = "Route stop list have stop that doesn't exist in the system")]
        public List<StopDto>? StopsRoute { get; set; } = null;
    }
}
