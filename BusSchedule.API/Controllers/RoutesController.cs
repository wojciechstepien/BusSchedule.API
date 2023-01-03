using AutoMapper;
using BusSchedule.API.Entities;
using BusSchedule.API.Models;
using BusSchedule.API.Models.ForCreation;
using BusSchedule.API.Models.ForUpdate;
using BusSchedule.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BusSchedule.API.Controllers
{
    [Route("api/route")]
    [ApiController]
    public class RoutesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IBusScheduleRepository _busScheduleRepository;
        private readonly ILogger<RoutesController> _logger;

        public RoutesController(IMapper mapper, IBusScheduleRepository busScheduleRepository, ILogger<RoutesController> logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _busScheduleRepository = busScheduleRepository ?? throw new ArgumentNullException(nameof(busScheduleRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        /// <summary>
        /// Get pointed route of bus
        /// </summary>
        /// <param name="routeId">Id of route to get</param>
        /// <returns>ActionResult with wraped RouteDto</returns>

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{routeId}", Name = "GetRoute")]
        public async Task<ActionResult<RouteDto>> GetRoute(int routeId)
        {
            try
            {
                var routeEntities = await _busScheduleRepository.GetRouteAsync(routeId);
                if (routeEntities == null)
                {
                    _logger.LogInformation($"Routes for pointed bus (id: {routeId}) did not exist in the database.");
                    return NotFound();
                }
                return Ok(new RouteDto
                {
                    Id = routeEntities.Id,
                    Bus = _mapper.Map<BusDto>(routeEntities.Bus),
                    StopsOrders = _mapper.Map<List<StopOrderDto>>(routeEntities.StopOrders)
                });
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting routes.", ex);
                return StatusCode(500, "Problem happend while handling your request.");
            }
        }
        /// <summary>
        /// Creates route for pointed bus
        /// </summary>
        /// <param name="busId">ID of the bus for which route will be created</param>
        /// <returns>An ActionResult with wraped newly created RouteDto </returns>

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<ActionResult<RouteDto>> CreateRoute(int busId)
        {
            try
            {
                if(!await _busScheduleRepository.BusExists(busId))
                {
                    return NotFound($"Can not create Route for pointed bus, because bus (id: {busId}) does not exist.");
                }
                var busForRoute = await _busScheduleRepository.GetBusAsync(busId);
                var newRoute = new Entities.Route { Bus= busForRoute };
                await _busScheduleRepository.AddRouteAsync(newRoute);
                await _busScheduleRepository.SaveChangesAsync();
                var createdRoute = _mapper.Map<RouteDto>(newRoute);
                return CreatedAtRoute("GetRoute", new { routeId = newRoute.Id}, createdRoute);
            }

            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while processing CreateRoute", ex);
                return StatusCode(500, "Problem happend while handling your request.");
            }
        }
        /// <summary>
        /// Adds Stop at pointed route and it's number order at this route
        /// </summary>
        /// <param name="routeId">ID of the route for which new stop and it's number order will be added</param>
        /// <param name="stopId">ID of the stop that will be added to route</param>
        /// <param name="orderNumber">stop number on the route (order number)</param>
        /// <returns>An ActionResult with wraped RouteDto with stops at this route</returns>
        
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("{routeId}/StopOrder")]
        public async Task<ActionResult<RouteDto>> CreateStopOrder(int routeId,int stopId, int orderNumber)
        {
            try
            {
                if (!await _busScheduleRepository.RouteExists(routeId))
                {
                    return NotFound($"Route (id: {routeId}) does not exist.");
                }
                if (!await _busScheduleRepository.StopExists(stopId))
                {
                    return NotFound($"Stop (id: {stopId}) does not exist.");
                }
                var newStopOrder = new StopOrder
                {
                    Stop = await _busScheduleRepository.GetStopAsync(stopId),
                    Order = orderNumber
                };
                await _busScheduleRepository.AddStopOrder(routeId,newStopOrder);
                await _busScheduleRepository.SaveChangesAsync();
                return CreatedAtRoute("GetRoute", new { routeId = routeId }, 
                    await _busScheduleRepository.GetRouteAsync(routeId));
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while processing CreateStopOrder", ex);
                return StatusCode(500, "Problem happend while handling your request.");
            }
        }
        /// <summary>
        /// Allows to Update stop and it's order of the route
        /// </summary>
        /// <param name="stopOrderId">ID of the route to update</param>
        /// <param name="stopOrder">StopOrderForUpdateDto of updated stop and orders</param>
        /// <returns>An ActionResult</returns>
        /// 
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("{routeId}/StopOrder")]
        public async Task<ActionResult> UpdateStopOrder(int stopOrderId,StopOrderForUpdateDto stopOrder)
        {
            if(!await _busScheduleRepository.StopOrderExists(stopOrderId))
            {
                return NotFound();
            }
            var stopOrderEntity = await _busScheduleRepository.GetStopOrderAsync(stopOrderId);
            _mapper.Map(stopOrder,stopOrderEntity);
            await _busScheduleRepository.SaveChangesAsync();
            return NoContent();
        }
        /// <summary>
        /// Deletes pointed route
        /// </summary>
        /// <param name="routeId">ID of the route to delete</param>
        /// <returns>An ActionResult</returns>
        /// 
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete]
        public async Task<ActionResult> DeleteRoute(int routeId)
        {
            if (!await _busScheduleRepository.RouteExists(routeId))
            {
                return NotFound();
            }
            var routeToDelete = await _busScheduleRepository.GetRouteAsync(routeId);
            if (routeToDelete == null)
            {
                return NotFound();
            }
            _busScheduleRepository.DeleteRoute(routeToDelete);
            await _busScheduleRepository.SaveChangesAsync();
            return NoContent();
        }
    }
}
