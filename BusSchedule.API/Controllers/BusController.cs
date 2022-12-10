using BusSchedule.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BusSchedule.API.Controllers
{
    [Route("api/stops/{stopId}/bus")]
    [ApiController]
    public class BusController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<BusDto>> GetStopBuses(int stopId)
        {
            var buses = StopsDataStore.Current.Stops.FirstOrDefault(s => s.Id == stopId);
            if (buses?.BusList == null)
            {
                return NotFound();
            }
            return Ok(buses.BusList);
        }

        [HttpGet("{busid}")]
        public ActionResult<BusDto> GetStopBus(int stopId,int busid)
        {
            var buses = StopsDataStore.Current.Stops.FirstOrDefault(s => s.Id == stopId);
            if (buses?.BusList == null)
            {
                return NotFound();
            }
            var stopbus = buses.BusList.FirstOrDefault(s => s.Id == busid);
            if (stopbus == null)
            {
                return NotFound();
            }
            return Ok(stopbus);
        }
    }
}
