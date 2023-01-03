using AutoMapper;
using BusSchedule.API.Entities;
using BusSchedule.API.Models;
using BusSchedule.API.Models.ForCreation;
using BusSchedule.API.Models.ForUpdate;
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
        /// <summary>
        /// Get All Stops
        /// </summary>
        /// <returns>An ActionResult with wraped IEnumerable of StopDto</returns>
        /// 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        /// <summary>
        /// Get pointed stop 
        /// </summary>
        /// <param name="stopId">ID of the bus to get</param>
        /// <returns>An ActionResult with wraped StopDto</returns>
        /// 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        /// <summary>
        /// Creates new stop
        /// </summary>
        /// <param name="stop">Stop to add</param>
        /// <returns>ActionResult</returns>
        /// 
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        /// <summary>
        /// Updates pointed stop
        /// </summary>
        /// <param name="stopId">ID of the stop to update</param>
        /// <param name="updatedStop">Updated stop</param>
        /// <returns>An ActionResult</returns>
        /// 
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("{stopId}")]
        public async Task<ActionResult> UpdateStop(int stopId, StopForUpdateDto updatedStop)
        {
            try
            {
                if (!await _busScheduleRepository.StopExists(stopId))
                {
                    _logger.LogInformation($"Pointed Stop (id: {stopId}) does not exist in data store)");
                    return NotFound();
                }
                var stopEntity = await _busScheduleRepository.GetStopAsync(stopId);
                _mapper.Map(updatedStop, stopEntity);
                await _busScheduleRepository.SaveChangesAsync();
                _logger.LogInformation($"Updated stop ({stopId})");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while processing UpdateStop({stopId})", ex);
                return StatusCode(500, "Problem happend while handling your request.");
            }
        }
        /// <summary>
        /// Deletes pointed Stop
        /// </summary>
        /// <param name="stopId">ID of the stop to delete</param>
        /// <returns>ActionResult</returns>
        /// 
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete]
        public async Task<ActionResult> DeleteStop(int stopId)
        {
            if (!await _busScheduleRepository.StopExists(stopId))
            {
                return NotFound();
            }
            var stopToDelete = await _busScheduleRepository.GetStopAsync(stopId);
            if (stopToDelete == null)
            {
                return NotFound();
            }
            _busScheduleRepository.DeleteStop(stopToDelete);
            await _busScheduleRepository.SaveChangesAsync();
            return NoContent();
        }
    }
}
