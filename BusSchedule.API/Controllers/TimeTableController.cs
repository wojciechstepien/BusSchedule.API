using BusSchedule.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace BusSchedule.API.Controllers
{
    [ApiController]
    [Route("api/timetable")]
    public class TimeTableController : ControllerBase
    {
        [HttpGet("{busId}")]
        public ActionResult<TimeTableDto> GetTimeTable(int busId)
        {
            var timeTable = TimeTablesDataStore.Instance.TimeTables.FirstOrDefault(s => s.Bus.Id == busId);
            if (timeTable == null)
            {
                return NotFound();
            }
            return Ok(timeTable);
        }
    }
}
