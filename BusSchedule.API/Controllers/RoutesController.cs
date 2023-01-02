using AutoMapper;
using BusSchedule.API.Entities;
using BusSchedule.API.Models;
using BusSchedule.API.Services;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("{busId}")]
        public async Task<ActionResult<RouteDto>> GetRoute(int busId)
        {
            try
            {
                var routeEntities = await _busScheduleRepository.GetBusRouteAsync(busId);
                if (routeEntities == null)
                {
                    _logger.LogInformation($"Routes for pointed bus (id: {busId}) did not exist in the database.");
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
    }
}
