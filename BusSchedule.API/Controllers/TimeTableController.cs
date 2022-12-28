using BusSchedule.API.Models;
using BusSchedule.API.Models.ForCreation;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BusSchedule.API.Controllers
{
    [ApiController]
    [Route("api/timetable")]
    public class TimeTableController : ControllerBase
    {
        // TODO: is the return result okay or should I return whole TimeTableDto?
        [HttpGet("{busId}")]
        public ActionResult<List<TimeTableDto>> GetBusTimeTables(int busId)
        {
            var timeTable = TimeTablesDataStore.Instance.TimeTables.FindAll(d => d.Bus.Id == busId);
            if (timeTable == null)
            {
                return NotFound();
            }
            return Ok(timeTable);

        }
        [HttpGet("{busId}/stop/{stopId}", Name = "GetStopAndBusTimetable")]
        public ActionResult<TimeTableDto> GetStopAndBusTimetable(int busId,int stopId)
        {
            var timeTable = TimeTablesDataStore.Instance.TimeTables.FirstOrDefault((p) =>
            {
                return p.Bus.Id == busId
                       && p.Stop.Id == stopId;
            });

            if (timeTable == null)
            {
                return NotFound();
            }
            return Ok(timeTable);
        }
        [HttpGet("stop/{stopId}")]
        public ActionResult<List<TimeTableDto>> GetAllStopTimeTables(int stopId)
        {
            var timeTable = TimeTablesDataStore.Instance.TimeTables.FindAll(c => c.Stop.Id == stopId);
            if (timeTable == null)
            {
                return NotFound();
            }
            return Ok(timeTable);
        }

        [HttpPost("{busId}/stop/{stopId}")]
        public ActionResult<TimeTableDto> AddTimetablesToBus(int busId, int stopId, List<TimeOnly> timeOnlys)
        {
            var timeTable = TimeTablesDataStore.Instance.TimeTables.FirstOrDefault((p) =>
            {
                return p.Bus.Id == busId
                       && p.Stop.Id == stopId;
            });
            if (timeTable == null)
            {
                return NotFound("Not found Timetable for pointed busId and stopId");
            }
            var created = false;
            foreach (var timeOnly in timeOnlys) 
            {
                if (!timeTable.Times.Contains(timeOnly))
                {
                    timeTable.Times.Add(timeOnly);
                    created= true;
                }
            }
            if (created) return CreatedAtRoute("GetStopAndBusTimetable", new { busId = busId , stopId = stopId }, timeTable);
            return NotFound("Time already exists");
        }
        [HttpPut("{busId}/stop/{stopId}")]
        public ActionResult UpdateTimeTable(int busId,int stopId,TimeTableDto newTimeTable)
        {
            var timetable = TimeTablesDataStore.Instance.TimeTables.FirstOrDefault((p) =>
            {
                return p.Bus.Id == busId
                       && p.Stop.Id == stopId;
            });
            if (timetable == null) return NotFound();
            timetable.Stop = newTimeTable.Stop;
            timetable.Bus = newTimeTable.Bus;
            timetable.Times.Clear();
            timetable.Times.AddRange(newTimeTable.Times);
            return NoContent();
        }

    }
}
