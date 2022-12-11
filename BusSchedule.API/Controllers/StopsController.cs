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


        [HttpGet("{stopId}", Name = "GetStop")]
        public ActionResult<StopDto> GetStop(int stopId) 
        {
            var stopToReturn = StopsDataStore.Instance.Stops.FirstOrDefault(c => c.Id == stopId);
            if (stopToReturn == null)
            {
                return NotFound();
            }
            return Ok(stopToReturn);
        }
        [HttpPost]
        public ActionResult<StopDto> CreateStop(string Name)
        {
            var stops = StopsDataStore.Instance.Stops;
            var stop = StopsDataStore.Instance.Stops.FirstOrDefault(s => s.Name == Name);
            if (stop == null)
            {
                var maxId = StopsDataStore.Instance.Stops.Max(c => c.Id);
                var newStop = new StopDto() { Id = ++maxId,Name = Name };
                stops.Add(newStop);
                return CreatedAtRoute("GetStop",new {stopId = newStop.Id},newStop);
            }
            return NotFound();
        }
    }
}
