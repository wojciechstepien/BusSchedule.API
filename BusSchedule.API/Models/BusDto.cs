using BusSchedule.API.Validation;
using System.ComponentModel.DataAnnotations;

namespace BusSchedule.API.Models
{
    public class BusDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [StopsInListExists(ErrorMessage = "Stops didn't exist in the system")]
        public List<StopDto>? StopsRoute { get; set; } = null;
    }
}
