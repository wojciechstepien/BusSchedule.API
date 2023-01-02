using AutoMapper;
using BusSchedule.API.Entities;
using BusSchedule.API.Models;
using BusSchedule.API.Models.ForCreation;
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

        //[HttpPut("{busId}")]
        //public ActionResult UpdateStop(int busId, BusForUpdateDto newBus)
        //{
        //    try
        //    {
        //        var bus = BusesDataStore.Instance.Buses.FirstOrDefault(c => c.Id == busId);
        //        if (bus == null)
        //        {
        //            _logger.LogInformation($"Pointed Bus (id: {busId}) does not exist in data store)");
        //            return NotFound();
        //        }
        //        bus.Name = newBus.Name;
        //        bus.StopsRoute.Clear();
        //        bus.StopsRoute.AddRange(newBus.StopsRoute);
        //        _logger.LogInformation($"Updated Bus ({busId})");
        //        return NoContent();
        //    }
        //    catch(Exception ex)
        //    {
        //        _logger.LogCritical($"Exception occured while processing UpdateBus({busId})", ex);
        //        return StatusCode(500, "Problem happend while handling your request.");
        //    }
        //}

        //[HttpDelete("{busId}")]
        //public ActionResult DeleteBus(int busId) 
        //{
        //    try
        //    {
        //        var bus = BusesDataStore.Instance.Buses.FirstOrDefault(c => c.Id == busId);
        //        if (bus == null)
        //        {
        //            _logger.LogInformation($"Pointed Bus (id: {busId}) does not exist in data store)");
        //            return NotFound();
        //        }
        //        BusesDataStore.Instance.Buses.Remove(bus);
        //        _logger.LogWarning($"Delete Bus ({busId})");
        //        return NoContent();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogCritical($"Exception occured while processing DeleteBus({busId})", ex);
        //        return StatusCode(500, "Problem happend while handling your request.");
        //    }
        //}
    }
}
