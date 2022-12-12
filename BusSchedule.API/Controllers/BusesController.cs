using BusSchedule.API.Models;
using BusSchedule.API.Models.ForCreation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BusSchedule.API.Controllers
{
    [Route("api/bus")]
    [ApiController]
    public class BusesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<BusDto>> GetBuses()
        {
            var buses = BusesDataStore.Instance.Buses;
            if (buses == null)
            {
                return NotFound();
            }
            return Ok(buses);
        }

        [HttpGet("{busId}", Name ="GetBus")]
        public ActionResult<BusDto> GetBus(int busId)
        {
            var bus = BusesDataStore.Instance.Buses.FirstOrDefault(s => s.Id == busId);
            if (bus == null)
            {
                return NotFound();
            }
            return Ok(bus);
        }

        [HttpGet("{busId}/route")]
        public ActionResult<List<StopDto>> GetBusRoute(int busId)
        {
            var bus = BusesDataStore.Instance.Buses.FirstOrDefault(s => s.Id == busId);
            if (bus?.StopsRoute == null)
            {
                return NotFound();
            }
            return Ok(bus.StopsRoute);
        }
        [HttpPost]
        public ActionResult<BusDto> CreateBus(BusForCreationDto bus) 
        {
            var maxId = BusesDataStore.Instance.Buses.Max(s => s.Id);
            var newBus = new BusDto { Id = ++maxId, Name = bus.Name, StopsRoute = bus.StopsRoute };
            BusesDataStore.Instance.Buses.Add(newBus);
            return CreatedAtRoute("GetBus", new { busId = maxId }, newBus);
        }
    }
}
