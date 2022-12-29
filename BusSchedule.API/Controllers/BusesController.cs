using BusSchedule.API.Models;
using BusSchedule.API.Models.ForCreation;
using BusSchedule.API.Models.ForUpdate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BusSchedule.API.Controllers
{
    [Route("api/bus")]
    [ApiController]
    public class BusesController : ControllerBase
    {
        private readonly ILogger<BusesController> _logger;
        public BusesController(ILogger<BusesController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        [HttpGet]
        public ActionResult<IEnumerable<BusDto>> GetBuses()
        {
            try
            {
                var buses = BusesDataStore.Instance.Buses;
                if (buses == null)
                {
                    _logger.LogInformation("Buses didn't exists in data store");
                    return NotFound();
                }
                return Ok(buses);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting buses", ex);
                return StatusCode(500, "Problem happend while handling your request.");
            }
        }

        [HttpGet("{busId}", Name ="GetBus")]
        public ActionResult<BusDto> GetBus(int busId)
        {
            try
            {
                var bus = BusesDataStore.Instance.Buses.FirstOrDefault(s => s.Id == busId);
                if (bus == null)
                {
                    _logger.LogInformation($"Pointed Bus (id: {busId}) does not exist in data store)");
                    return NotFound();
                }
                return Ok(bus);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while processing GetBus({busId}", ex);
                return StatusCode(500, "Problem happend while handling your request.");
            }
        }

        [HttpGet("{busId}/route")]
        public ActionResult<List<StopDto>> GetBusRoute(int busId)
        {
            try
            {
                var bus = BusesDataStore.Instance.Buses.FirstOrDefault(s => s.Id == busId);
                if (bus?.StopsRoute == null)
                {
                    _logger.LogInformation($"Pointed Bus Route (id: {busId}) does not exist in data store)");
                    return NotFound();
                }
                return Ok(bus.StopsRoute);
            }
            catch   (Exception ex) 
            {
                _logger.LogCritical($"Exception occured while processing GetBusRoute({busId}", ex);
                return StatusCode(500, "Problem happend while handling your request.");
            }
        }
        [HttpPost]
        public ActionResult<BusDto> CreateBus(BusForCreationDto bus) 
        {
            try
            {
                var maxId = BusesDataStore.Instance.Buses.Max(s => s.Id);
                var newBus = new BusDto { Id = ++maxId, Name = bus.Name, StopsRoute = bus.StopsRoute };
                BusesDataStore.Instance.Buses.Add(newBus);
                _logger.LogInformation("Created new bus");
                return CreatedAtRoute("GetBus", new { busId = maxId }, newBus);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while processing CreateBus", ex);
                return StatusCode(500, "Problem happend while handling your request.");
            }
        }
        [HttpPut("{busId}")]
        public ActionResult UpdateStop(int busId, BusForUpdateDto newBus)
        {
            try
            {
                var bus = BusesDataStore.Instance.Buses.FirstOrDefault(c => c.Id == busId);
                if (bus == null)
                {
                    _logger.LogInformation($"Pointed Bus (id: {busId}) does not exist in data store)");
                    return NotFound();
                }
                bus.Name = newBus.Name;
                bus.StopsRoute.Clear();
                bus.StopsRoute.AddRange(newBus.StopsRoute);
                _logger.LogInformation($"Updated Bus ({busId})");
                return NoContent();
            }
            catch(Exception ex)
            {
                _logger.LogCritical($"Exception occured while processing UpdateBus({busId})", ex);
                return StatusCode(500, "Problem happend while handling your request.");
            }
        }
        [HttpDelete("{busId}")]
        public ActionResult DeleteBus(int busId) 
        {
            try
            {
                var bus = BusesDataStore.Instance.Buses.FirstOrDefault(c => c.Id == busId);
                if (bus == null)
                {
                    _logger.LogInformation($"Pointed Bus (id: {busId}) does not exist in data store)");
                    return NotFound();
                }
                BusesDataStore.Instance.Buses.Remove(bus);
                _logger.LogWarning($"Delete Bus ({busId})");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while processing DeleteBus({busId})", ex);
                return StatusCode(500, "Problem happend while handling your request.");
            }
        }
    }
}
