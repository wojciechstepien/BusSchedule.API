using BusSchedule.API.Models;
using BusSchedule.API.Models.ForCreation;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BusSchedule.API.Controllers
{
    [ApiController]
    [Route("api/timetable")]
    public class TimeTableController : ControllerBase
    {
        private readonly ILogger<TimeTableController> _logger;
        public TimeTableController(ILogger<TimeTableController> logger)
        { 
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        // TODO: is the return result okay or should I return whole TimeTableDto?
        [HttpGet("{busId}")]
        public ActionResult<List<TimeTableDto>> GetBusTimeTables(int busId)
        {
            try
            {
                var timeTable = TimeTablesDataStore.Instance.TimeTables.FindAll(d => d.Bus.Id == busId);
                if (timeTable == null)
                {
                    return NotFound();
                }
                return Ok(timeTable);
            }
            catch(Exception ex)
            {
                _logger.LogCritical($"Exception while getting bus timetables ({busId})", ex);
                return StatusCode(500, "Problem happend while handling your request.");
            }

        }
        [HttpGet("{busId}/stop/{stopId}", Name = "GetStopAndBusTimetable")]
        public ActionResult<TimeTableDto> GetStopAndBusTimetable(int busId,int stopId)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting timetable (stopid: {stopId},busid: {busId})", ex);
                return StatusCode(500, "Problem happend while handling your request.");
            }
        }

        [HttpGet("stop/{stopId}")]
        public ActionResult<List<TimeTableDto>> GetAllStopTimeTables(int stopId)
        {
            try
            {
                var timeTable = TimeTablesDataStore.Instance.TimeTables.FindAll(c => c.Stop.Id == stopId);
                if (timeTable == null)
                {
                    return NotFound();
                }
                return Ok(timeTable);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while stop timetables ({stopId})", ex);
                return StatusCode(500, "Problem happend while handling your request.");
            }
        }

        [HttpPost("{busId}/stop/{stopId}")]
        public ActionResult<TimeTableDto> AddTimetablesToBus(int busId, int stopId, List<TimeOnly> timeOnlys)
        {
            try
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
                        created = true;
                    }
                }
                if (created) return CreatedAtRoute("GetStopAndBusTimetable", new { busId = busId, stopId = stopId }, timeTable);
                return NotFound("Time already exists");
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while adding timetables to bus ({busId}) at stop ({stopId}) ", ex);
                return StatusCode(500, "Problem happend while handling your request.");
            }
        }

        [HttpPut("{busId}/stop/{stopId}")]
        public ActionResult UpdateTimeTable(int busId,int stopId,TimeTableDto newTimeTable)
        {
            try
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
            catch(Exception ex)
            {
                _logger.LogCritical($"Exception while updating bus ({busId}) at stop ({stopId})", ex);
                return StatusCode(500, "Problem happend while handling your request.");
            }
        }

        [HttpPatch("{busId}/stop/{stopId}")]
        public ActionResult PartiallyUpdateTimeTable(int busId,int stopId,JsonPatchDocument<TimeTableDto> patchDocument) 
        {
            try
            {
                var timetable = TimeTablesDataStore.Instance.TimeTables.FirstOrDefault((p) =>
                {
                    return p.Bus.Id == busId
                           && p.Stop.Id == stopId;
                });
                if (timetable == null) return NotFound();
                var timeTableToPatch = new TimeTableDto
                {
                    Bus = timetable.Bus,
                    Stop = timetable.Stop,
                    Times = timetable.Times
                };
                patchDocument.ApplyTo(timeTableToPatch, ModelState);

                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                if (!TryValidateModel(timeTableToPatch))
                {
                    return BadRequest();
                }
                timetable.Bus = timeTableToPatch.Bus;
                timetable.Stop = timeTableToPatch.Stop;
                timetable.Times = timeTableToPatch.Times;

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while partially updating bus ({busId}) timetable at stop({stopId})", ex);
                return StatusCode(500, "Problem happend while handling your request.");
            }
        }
    }
}
