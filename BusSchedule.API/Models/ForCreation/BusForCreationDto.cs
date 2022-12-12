using BusSchedule.API.Validation;
using System.ComponentModel.DataAnnotations;

namespace BusSchedule.API.Models.ForCreation
{
    public class BusForCreationDto
    {
        [Required]
        public string? Name { get; set; } = string.Empty;
        [StopsInListExists(ErrorMessage = "Route stop list have stop that doesn't exist in the system")]
        public List<StopDto>? StopsRoute { get; set; } = null;
    }
}
