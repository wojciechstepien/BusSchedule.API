using AutoMapper;
using BusSchedule.API.Entities;
using BusSchedule.API.Models;
using BusSchedule.API.Models.ForCreation;
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
                return new RouteDto
                {
                    Id = routeEntities.Id,
                    Bus = _mapper.Map<BusDto>(routeEntities.Bus),
                    StopsOrders = _mapper.Map<List<StopOrderDto>>(routeEntities.StopOrders)
                };
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting routes.", ex);
                return StatusCode(500, "Problem happend while handling your request.");
            }
        }

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
    }
}
