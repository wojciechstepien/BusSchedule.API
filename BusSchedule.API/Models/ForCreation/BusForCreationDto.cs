using System.ComponentModel.DataAnnotations;

namespace BusSchedule.API.Models.ForCreation
{
    public class BusForCreationDto
    {
        [Required]
        public string? Name { get; set; } = string.Empty;
    }
}
