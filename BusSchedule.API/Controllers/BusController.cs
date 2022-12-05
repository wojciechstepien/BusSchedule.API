using BusSchedule.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BusSchedule.API.Controllers
{
    [Route("api/stops/{id}/bus")]
    [Route("api/bus")]
    [ApiController]
    public class BusController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<BusDto>> GetStopBus(int id)
        {
            var buses = StopsDataStore.Current.Stops.FirstOrDefault(s => s.Id == id);
            if(buses?.BusList == null)
            {
                return NotFound();
            }
            return Ok(buses.BusList);
        }
    }
}
