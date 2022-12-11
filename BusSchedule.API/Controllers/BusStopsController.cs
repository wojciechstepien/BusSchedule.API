using BusSchedule.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace BusSchedule.API.Controllers
{
    [ApiController]
    [Route("api/stops")]
    public class StopsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<StopDto>> GetBusStops()
        {
            return Ok(StopsDataStore.Current.Stops);
        }


        [HttpGet("{id}")]
        public ActionResult<StopDto> GetBusStop(int id)
        {
            var stopToReturn = StopsDataStore.Current.Stops.FirstOrDefault(c => c.Id == id);
            if (stopToReturn == null)
            {
                return NotFound();
            }
            return Ok(stopToReturn);
        }
    }
}
