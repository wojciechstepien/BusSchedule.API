using BusSchedule.API.Entities;

namespace BusSchedule.API.Services
{
    public interface IBusScheduleRepository
    {
        Task<Bus?> GetBusAsync(int busId);
        Task<IEnumerable<Bus>> GetBusesAsync();
        Task<Stop?> GetStopAsync(int stopId);
        Task<IEnumerable<Stop>> GetStopsAsync();
        Task<Entities.Route?> GetBusRouteAsync(int busId);
        Task<IEnumerable<TimeTable?>> GetBusAtStopTimetableAsync(int busId, int stopId);
    }
}
