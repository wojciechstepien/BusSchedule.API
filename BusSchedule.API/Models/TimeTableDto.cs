using BusSchedule.API.Models.ForUpdate;
using BusSchedule.API.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BusSchedule.API.Models
{
    public class TimeTableDto
    {
        [Required]
        public BusDto? Bus { get; set; }
        [Required]
        [StopExists(ErrorMessage = "That stop doesn't exist in the system")]
        public StopDto? Stop { get; set; }
        public List<TimeOnly>? Times { get; set; } = null;
    }
}
