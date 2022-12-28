using System.ComponentModel.DataAnnotations;

namespace BusSchedule.API.Models
{
    public class StopDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
