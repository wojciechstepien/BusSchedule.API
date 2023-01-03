using AutoMapper;
using BusSchedule.API.Entities;
using BusSchedule.API.Models;
using BusSchedule.API.Models.ForCreation;
using BusSchedule.API.Models.ForUpdate;
using BusSchedule.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BusSchedule.API.Controllers
{
    [Route("api/bus")]
    [ApiController]
    public class BusesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IBusScheduleRepository _busScheduleRepository;
        private readonly ILogger<BusesController> _logger;

        public BusesController(IBusScheduleRepository busScheduleRepository, IMapper mapper, ILogger<BusesController> logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _busScheduleRepository = busScheduleRepository ?? throw new ArgumentNullException(nameof(busScheduleRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        /// <summary>
        /// Get all Buses
        /// </summary>
        /// <returns>An IActionResult</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BusDto>>> GetBuses()
        {
            try
            {
                var busEntities = await _busScheduleRepository.GetBusesAsync();
                if (busEntities == null)
                {
                    _logger.LogInformation("Buses didn't exists in database");
                    return NotFound();
                }
                return Ok(_mapper.Map<IEnumerable<BusDto>>(busEntities));
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting buses", ex);
                return StatusCode(500, "Problem happend while handling your request.");
            }
        }
        /// <summary>
        /// Get bus of pointed id
        /// </summary>
        /// <param name="busId">Id of the bus that we want to return</param>
        /// <returns>ActionResult with wraped BusDto</returns>
        [HttpGet("{busId}", Name = "GetBus")]
        public async Task<ActionResult<BusDto>> GetBus(int busId)
        {
            try
            {
                var busEntity = await _busScheduleRepository.GetBusAsync(busId);
                if (busEntity == null)
                {
                    _logger.LogInformation($"Pointed Bus (id: {busId}) does not exist in data store)");
                    return NotFound();
                }
                return Ok(_mapper.Map<Bus>(busEntity));
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while processing GetBus({busId}", ex);
                return StatusCode(500, "Problem happend while handling your request.");
            }
        }
        /// <summary>
        /// Creates new bus
        /// </summary>
        /// <param name="bus">Bus to Create</param>
        /// <returns>An ActionResult with wraped created Bus </returns>
        [HttpPost]
        public async Task<ActionResult<BusDto>> CreateBus(BusForCreationDto bus)
        {
            try
            {
                var newBus = _mapper.Map<Bus>(bus);
                await _busScheduleRepository.AddBusAsync(newBus);
                await _busScheduleRepository.SaveChangesAsync();
                var createdBus = _mapper.Map<BusDto>(newBus);
                return CreatedAtRoute("GetBus", new { busId = createdBus.Id }, createdBus);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while processing CreateBus", ex);
                return StatusCode(500, "Problem happend while handling your request.");
            }
        }
        /// <summary>
        /// Updates already existed bus
        /// </summary>
        /// <param name="busId">Id of bus to update</param>
        /// <param name="updatedBus">Bus to Update</param>
        /// <returns></returns>
        [HttpPut("{busId}")]
        public async Task<ActionResult> UpdateBus(int busId, BusForUpdateDto updatedBus)
        {
            try
            {
                if (!await _busScheduleRepository.BusExists(busId))
                {
                    _logger.LogInformation($"Pointed Bus (id: {busId}) does not exist in data store)");
                    return NotFound();
                }
                var busEntity = await _busScheduleRepository.GetBusAsync(busId);
                _mapper.Map(updatedBus, busEntity);
                await _busScheduleRepository.SaveChangesAsync();
                _logger.LogInformation($"Updated Bus ({busId})");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while processing UpdateBus({busId})", ex);
                return StatusCode(500, "Problem happend while handling your request.");
            }
        }
        /// <summary>
        /// Deletes pointed Bus
        /// </summary>
        /// <param name="busId">Id of bus to delete</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ActionResult> DeleteBus(int busId)
        {
            if (!await _busScheduleRepository.BusExists(busId))
            {
                return NotFound();
            }
            var busToDelete = await _busScheduleRepository.GetBusAsync(busId);
            if (busToDelete == null)
            {
                return NotFound();
            }
            _busScheduleRepository.DeleteBus(busToDelete);
            await _busScheduleRepository.SaveChangesAsync();
            return NoContent();
        }
    }
}
