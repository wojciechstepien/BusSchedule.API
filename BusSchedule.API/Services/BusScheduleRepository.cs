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

        public async Task AddBusAsync(Bus bus)
        {
            if (bus != null)
            {
                await _context.Buses.AddAsync(bus);
            }
        }

        public async Task AddRouteAsync(Entities.Route route)
        {
            await _context.AddAsync(route);
        }

        public async Task AddStopAsync(Stop stop)
        {
            if (stop != null)
            {
                await _context.Stops.AddAsync(stop);
            }
        }

        public async Task AddTimeTable(TimeTable timeTable)
        {
            if (timeTable != null)
            {
                await _context.TimeTables.AddAsync(timeTable);
            }
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

        public async Task<Entities.Route?> GetBusRouteAsync(int routeId)
        {
            return await _context.Routes
                .Include(p => p.Bus)
                .Include(p => p.StopOrders)
                .ThenInclude(p=> p.Stop)
                .FirstOrDefaultAsync(c => c.Id == routeId);
        }
        public async Task<Stop?> GetStopAsync(int stopId)
        {
            return await _context.Stops.FirstOrDefaultAsync(c => c.Id == stopId);
        }

        public async Task<IEnumerable<Stop>> GetStopsAsync()
        {
            return await _context.Stops.ToListAsync();
        }

        public async Task<bool> RouteExists(int routeId)
        {
            return await _context.Routes.AnyAsync(c=>c.Id == routeId);
        }

        public async Task<bool> BusExists(int busId)
        {
            return await _context.Buses.AnyAsync(c => c.Id == busId);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 1);
        }

        public async Task AddStopOrder(int routeId,int stopId, int orderNumber)
        {
            
        }

        public async Task AddStopOrder(int routeId,StopOrder stopOrder)
        {
            var routes = _context.Routes.FirstOrDefault(c => c.Id == routeId);
            routes.StopOrders.Add(stopOrder);
        }

        public async Task<bool> StopExists(int stopId)
        {
            return (await _context.Stops.AnyAsync(c => c.Id == stopId));
        }
    }
}
