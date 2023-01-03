using AutoMapper;
using BusSchedule.API.Entities;
using BusSchedule.API.Models;
using BusSchedule.API.Models.ForCreation;
using BusSchedule.API.Models.ForUpdate;
using BusSchedule.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BusSchedule.API.Controllers
{
    [ApiController]
    [Route("api/timetable")]
    public class TimeTableController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IBusScheduleRepository _busScheduleRepository;
        private readonly ILogger<TimeTableController> _logger;
        public TimeTableController(ILogger<TimeTableController> logger, IBusScheduleRepository busScheduleRepository, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _busScheduleRepository = busScheduleRepository ?? throw new ArgumentNullException(nameof(busScheduleRepository));
        }
        /// <summary>
        /// Gets departure times of the pointed bus at pointed stop
        /// </summary>
        /// <param name="busId">ID of the bus</param>
        /// <param name="stopId">ID of the stop</param>
        /// <returns>Action Result with wraped list of timetablesdto</returns>
        [HttpGet(Name = "GetBusTimeTablesAtStop")]
        public async Task<ActionResult<List<TimeTableDto>>> GetBusTimeTablesAtStop(int busId, int stopId)
        {
            try
            {
                if (!await _busScheduleRepository.BusExists(busId))
                {
                    return NotFound($"Bus (id: {busId}) does not exist.");
                }
                if (!await _busScheduleRepository.StopExists(stopId))
                {
                    return NotFound($"Stop (id: {stopId}) does not exist.");
                }
                var timetableEntitites = await _busScheduleRepository.GetBusTimetableAtStopAsync(busId, stopId);
                if (timetableEntitites == null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<IEnumerable<TimeTableDto>>(timetableEntitites));

            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting bus ({busId}) timetables at stop ({stopId})", ex);
                return StatusCode(500, "Problem happend while handling your request.");
            }
        }
        /// <summary>
        /// Gets all bus departure times at pointed stop
        /// </summary>
        /// <param name="stopId">Id of the stop of which timetables will be returned</param>
        /// <returns>ActionResult with wraped list of TimeTableDto</returns>
        [HttpGet("{stopId}", Name = "GetTimeTablesAtStop")]
        public async Task<ActionResult<List<TimeTableDto>>> GetTimeTablesAtStop(int stopId)
        {
            try
            {
                if (!await _busScheduleRepository.StopExists(stopId))
                {
                    return NotFound($"Stop (id: {stopId}) does not exist.");
                }
                var timetableEntitites = await _busScheduleRepository.GetTimetableAtStopAsync(stopId);
                if (timetableEntitites == null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<IEnumerable<TimeTableDto>>(timetableEntitites));

            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting timetables at stop ({stopId})", ex);
                return StatusCode(500, "Problem happend while handling your request.");
            }
        }
        /// <summary>
        /// Adds departure time of pointed bus at pointed stop
        /// </summary>
        /// <param name="busId">Id of the bus</param>
        /// <param name="stopId">Id of the stop</param>
        /// <param name="time">time of departure to add</param>
        /// <returns>An ActionResult with wraped newly created timetable</returns>
        [HttpPost]
        public async Task<ActionResult<TimeTableDto>> CreateTimeTable(int busId, int stopId, TimeOnly time)
        {
            try
            {
                if (!await _busScheduleRepository.BusExists(busId))
                {
                    return NotFound($"Bus (id: {busId}) does not exist.");
                }
                if (!await _busScheduleRepository.StopExists(stopId))
                {
                    return NotFound($"Stop (id: {stopId}) does not exist.");
                }
                var timeTableToAdd = new TimeTable
                {
                    Bus = await _busScheduleRepository.GetBusAsync(busId),
                    Stop = await _busScheduleRepository.GetStopAsync(stopId),
                    Time = time
                };
                await _busScheduleRepository.AddTimeTable(timeTableToAdd);
                await _busScheduleRepository.SaveChangesAsync();
                return CreatedAtRoute("GetBusTimeTablesAtStop", new { busId = busId, stopId = stopId }, _mapper.Map<TimeTableDto>(timeTableToAdd));
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while creating timetable for bus ({busId}) at stop ({stopId})", ex);
                return StatusCode(500, "Problem happend while handling your request.");
            }
        }
        /// <summary>
        /// Updates timetable
        /// </summary>
        /// <param name="timeTableId">ID of the TimeTable to update</param>
        /// <param name="timeTableForUpdate">TimeTable to update</param>
        /// <returns>ActionResult</returns>
        [HttpPut]
        public async Task<ActionResult> UpdateTime(int timeTableId, TimeTableForUpdateDto timeTableForUpdate)
        {
            if (!await _busScheduleRepository.TimeTableExists(timeTableId))
            {
                return NotFound($"Bus (id: {timeTableId}) does not exist.");
            }
            var timeTableEntity = await _busScheduleRepository.GetTimeTableAsync(timeTableId);
            _mapper.Map(timeTableForUpdate, timeTableEntity);
            await _busScheduleRepository.SaveChangesAsync();
            return NoContent();
        }
        /// <summary>
        /// Deletes pointed timetable
        /// </summary>
        /// <param name="timeTableId">ID of the timetable to delete</param>
        /// <returns>ActionResult</returns>
        [HttpDelete]
        public async Task<ActionResult> DeleteTimeTable(int timeTableId)
        {
            if(!await _busScheduleRepository.TimeTableExists(timeTableId))
            {
                return NotFound();
            }
            var timeTableToDelete = await _busScheduleRepository.GetTimeTableAsync(timeTableId);
            if(timeTableToDelete == null) 
            {
                return NotFound();
            }
            _busScheduleRepository.DeleteTimeTable(timeTableToDelete);
            await _busScheduleRepository.SaveChangesAsync();
            return NoContent();
        }
    }
}
