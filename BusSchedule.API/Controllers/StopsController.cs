using AutoMapper;
using BusSchedule.API.Entities;
using BusSchedule.API.Models;
using BusSchedule.API.Models.ForCreation;
using BusSchedule.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BusSchedule.API.Controllers
{
    [ApiController]
    [Route("api/stops")]
    public class StopsController : ControllerBase
    {
        private readonly ILogger<StopsController> _logger;
        private readonly IBusScheduleRepository _busScheduleRepository;
        private readonly IMapper _mapper;

        public StopsController(ILogger<StopsController> logger,IBusScheduleRepository busScheduleRepository,IMapper mapper) 
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _busScheduleRepository = busScheduleRepository ?? throw new ArgumentNullException(nameof(busScheduleRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StopDto>>> GetStops()
        {
            try
            {
                var stopEntities = await _busScheduleRepository.GetStopsAsync();
                if (stopEntities == null)
                {
                    _logger.LogInformation($"Stops did not exist in database");
                    return NotFound();
                }
                return Ok(_mapper.Map<IEnumerable<StopDto>>(stopEntities));
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while getting stops", ex);
                return StatusCode(500, "Problem happend while handling your request.");
            }
        }

        [HttpGet("{stopId}", Name = "GetStop")]
        public async Task<ActionResult<StopDto>> GetStop(int stopId) 
        {
            try
            {
                var stopEntity = await _busScheduleRepository.GetStopAsync(stopId);
                if (stopEntity == null)
                {
                    _logger.LogInformation($"Pointed stop (id: {stopId}) does not exist in database)");
                    return NotFound();
                }
                return Ok(_mapper.Map<StopDto>(stopEntity));
            }
            catch(Exception ex)
            {
                _logger.LogCritical($"Exception occured while getting stop ({stopId})", ex);
                return StatusCode(500, "Problem happend while handling your request.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<StopDto>> CreateStop(StopForCreationDto stop)
        {
            try
            {
                var newStop = _mapper.Map<Stop>(stop);
                await _busScheduleRepository.AddStopAsync(newStop);
                await _busScheduleRepository.SaveChangesAsync();
                var createdStop = _mapper.Map<StopDto>(newStop);
                return CreatedAtRoute("GetStop", new { stopId = createdStop.Id }, createdStop);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while processing CreateStop", ex);
                return StatusCode(500, "Problem happend while handling your request.");
            }
        }

        //[HttpPut("{stopId}")]
        //public ActionResult UpdateStop(int stopId, StopForUpdateDto newStop)
        //{
        //    try
        //    {
        //        var stop = StopsDataStore.Instance.Stops.FirstOrDefault(c => c.Id == stopId);
        //        if (stop == null)
        //        {
        //            return NotFound();
        //        }
        //        stop.Name = newStop.Name;
        //        return NoContent();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogCritical($"Exception while updating bus ({stopId})", ex);
        //        return StatusCode(500, "Problem happend while handling your request.");
        //    }
        //}

        //[HttpDelete("{stopId}")]
        //public ActionResult DeleteStop(int stopId)
        //{
        //    try
        //    {
        //        var stop = StopsDataStore.Instance.Stops.FirstOrDefault(c => c.Id == stopId);
        //        if (stop == null) return NotFound();
        //        StopsDataStore.Instance.Stops.Remove(stop);
        //        return NoContent();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogCritical($"Exception while deleting bus ({stopId})", ex);
        //        return StatusCode(500, "Problem happend while handling your request.");
        //    }
        //}
    }
}
