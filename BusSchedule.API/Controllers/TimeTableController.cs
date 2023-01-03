using AutoMapper;
using BusSchedule.API.Entities;
using BusSchedule.API.Models;
using BusSchedule.API.Models.ForCreation;
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
            _busScheduleRepository= busScheduleRepository ?? throw new ArgumentNullException(nameof(busScheduleRepository));
        }

        [HttpGet(Name = "GetBusTimeTablesAtStop")]
        public async Task<ActionResult<List<TimeTableDto>>> GetBusTimeTablesAtStop(int busId,int stopId)
        {
            try
            {
                if (!await _busScheduleRepository.BusExists(busId))
                {
                    return NotFound($"Bus (id: {busId}) does not exist.");
                }
                if(!await _busScheduleRepository.StopExists(stopId))
                {
                    return NotFound($"Stop (id: {stopId}) does not exist.");
                }
                var timetableEntitites = await _busScheduleRepository.GetBusTimetableAtStopAsync(busId, stopId);
                if(timetableEntitites == null )
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<IEnumerable<TimeTableDto>>(timetableEntitites));

            }
            catch(Exception ex)
            {
                _logger.LogCritical($"Exception while getting bus ({busId}) timetables at stop ({stopId})", ex);
                return StatusCode(500, "Problem happend while handling your request.");
            }
        }
        [HttpGet("{stopId}", Name = "GetTimeTablesAtStop")]
        public async Task<ActionResult<List<TimeTableDto>>> GetTimeTablesAtStop( int stopId)
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
        [HttpPost]
        public async Task<ActionResult<TimeTableDto>> CreateTimeTable(int busId,int stopId, TimeOnly time)
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
    }
}
