using BusSchedule.API.Models.ForUpdate;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BusSchedule.API.Models
{
    public class TimeTableDto
    {
        public int Id { get; set; }
        public BusDto? Bus { get; set; }
        public StopDto? Stop { get; set; }
        public TimeOnly Time { get; set; }
    }
}
