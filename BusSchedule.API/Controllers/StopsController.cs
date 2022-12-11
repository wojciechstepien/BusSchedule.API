using BusSchedule.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace BusSchedule.API.Controllers
{
    [ApiController]
    [Route("api/stops")]
    public class StopsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<StopDto>> GetStops()
        {
            return Ok(StopsDataStore.Instance.Stops);
        }


        [HttpGet("{stopId}")]
        public ActionResult<StopDto> GetStop(int stopId) 
        {
            var stopToReturn = StopsDataStore.Instance.Stops.FirstOrDefault(c => c.Id == stopId);
            if (stopToReturn == null)
            {
                return NotFound();
            }
            return Ok(stopToReturn);
        }
    }
}
