using BusSchedule.API.Converters;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace BusSchedule.API.Models
{
    public class TimeTableDto
    {
        public BusDto? Bus { get; set; }
        public StopDto? Stop { get; set; }
        public List<TimeOnly>? Times { get; set; } = null;
    }
}
