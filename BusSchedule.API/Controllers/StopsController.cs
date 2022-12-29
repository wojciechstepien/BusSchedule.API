using BusSchedule.API.Models;
using BusSchedule.API.Models.ForUpdate;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BusSchedule.API.Controllers
{
    [ApiController]
    [Route("api/stops")]
    public class StopsController : ControllerBase
    {
        private readonly ILogger<StopsController> _logger;
        public StopsController(ILogger<StopsController> logger) 
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public ActionResult<IEnumerable<StopDto>> GetStops()
        {
            try
            {
                return Ok(StopsDataStore.Instance.Stops);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting stops", ex);
                return StatusCode(500, "Problem happend while handling your request.");
            }
        }


        [HttpGet("{stopId}", Name = "GetStop")]
        public ActionResult<StopDto> GetStop(int stopId) 
        {
            try
            {
                var stopToReturn = StopsDataStore.Instance.Stops.FirstOrDefault(c => c.Id == stopId);
                if (stopToReturn == null)
                {
                    _logger.LogInformation($"Pointed Bus (id: {stopId}) does not exist in data store)");
                    return NotFound();
                }
                return Ok(stopToReturn);
            }
            catch(Exception ex)
            {
                _logger.LogCritical($"Exception while getting bus ({stopId})", ex);
                return StatusCode(500, "Problem happend while handling your request.");
            }
        }
        [HttpPost]
        public ActionResult<StopDto> CreateStop(string Name)
        {
            try
            {
                var stops = StopsDataStore.Instance.Stops;
                var stop = StopsDataStore.Instance.Stops.FirstOrDefault(s => s.Name == Name);
                if (stop == null)
                {
                    var maxId = StopsDataStore.Instance.Stops.Max(c => c.Id);
                    var newStop = new StopDto() { Id = ++maxId, Name = Name };
                    stops.Add(newStop);
                    _logger.LogInformation($"Created new bus ({newStop.Id})");
                    return CreatedAtRoute("GetStop", new { stopId = newStop.Id }, newStop);
                }
                return NotFound();
            } catch(Exception ex)
            {
                _logger.LogCritical($"Exception while creating bus", ex);
                return StatusCode(500, "Problem happend while handling your request.");
            }
        }
        [HttpPut("{stopId}")]
        public ActionResult UpdateStop(int stopId, StopForUpdateDto newStop)
        {
            try
            {
                var stop = StopsDataStore.Instance.Stops.FirstOrDefault(c => c.Id == stopId);
                if (stop == null)
                {
                    return NotFound();
                }
                stop.Name = newStop.Name;
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while updating bus ({stopId})", ex);
                return StatusCode(500, "Problem happend while handling your request.");
            }
        }

        [HttpDelete("{stopId}")]
        public ActionResult DeleteStop(int stopId)
        {
            try
            {
                var stop = StopsDataStore.Instance.Stops.FirstOrDefault(c => c.Id == stopId);
                if (stop == null) return NotFound();
                StopsDataStore.Instance.Stops.Remove(stop);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while deleting bus ({stopId})", ex);
                return StatusCode(500, "Problem happend while handling your request.");
            }
        }
    }
}
