using BusSchedule.API.Entities;

namespace BusSchedule.API.Services
{
    public interface IBusScheduleRepository
    {
        Task<Bus> GetBusAsync(int busId);
        Task<IEnumerable<Bus>> GetBusesAsync();
        Task<Stop?> GetStopAsync(int stopId);
        Task<IEnumerable<Stop>> GetStopsAsync();
        Task<Entities.Route?> GetRouteAsync(int busId);
        Task<StopOrder?> GetStopOrderAsync(int stopOrderId);
        Task<TimeTable?> GetTimeTableAsync(int timeTableId);
        Task<IEnumerable<TimeTable?>> GetBusTimetableAtStopAsync(int busId, int stopId);
        Task<IEnumerable<TimeTable?>> GetTimetableAtStopAsync(int stopId);
        Task AddBusAsync(Bus bus);
        Task AddStopAsync(Stop stop);
        Task AddRouteAsync(Entities.Route route);
        Task AddStopOrder(int routeId,StopOrder stopOrder);
        Task AddTimeTable(TimeTable timeTable);
        Task<bool> SaveChangesAsync();
        Task<bool> RouteExists(int routeId);
        Task<bool> BusExists(int busId);
        Task<bool> StopExists(int stopId);
        Task<bool> StopOrderExists(int stopOrderId);
        Task<bool> TimeTableExists(int timeTableId);
        void DeleteTimeTable(TimeTable timeTableToDelete);
        void DeleteBus(Bus busToDelete);
        void DeleteStop(Stop stopToDelete);
        void DeleteRoute(Entities.Route routeToDelete);
    }
}
