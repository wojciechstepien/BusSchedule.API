using BusSchedule.API.DbContext;
using BusSchedule.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusSchedule.API.Services
{
    public class BusScheduleRepository : IBusScheduleRepository
    {
        private readonly BusScheduleContext _context;

        public BusScheduleRepository(BusScheduleContext context) 
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<Bus?> GetBusAsync(int busId)
        {
            return await _context.Buses.FirstOrDefaultAsync(c => c.Id == busId);
        }
        
        public async Task<IEnumerable<TimeTable?>> GetBusAtStopTimetableAsync(int busId, int stopId)
        {
            return await _context.TimeTables.Include(p => p.Stop).Include(p => p.Bus)
                .Where(p => (p.Bus.Id == busId) && (p.Stop.Id == stopId))
                .OrderBy(p => p.Time)
                .ToListAsync();
        }

        public async Task<IEnumerable<Bus>> GetBusesAsync()
        {
            return await _context.Buses.ToListAsync();
        }

        public async Task<Entities.Route?> GetBusRouteAsync(int busId)
        {
            return await _context.Routes
                .Include(p => p.Bus)
                .Include(p => p.StopOrders)
                .ThenInclude(p=> p.Stop)
                .FirstOrDefaultAsync(c => c.Bus.Id == busId);
        }
        public async Task<Stop?> GetStopAsync(int stopId)
        {
            return await _context.Stops.FirstOrDefaultAsync(c => c.Id == stopId);
        }

        public async Task<IEnumerable<Stop>> GetStopsAsync()
        {
            return await _context.Stops.ToListAsync();
        }
    }
}
