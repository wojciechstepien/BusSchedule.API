
using BusSchedule.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace BusSchedule.API.DbContext
{
    public class BusScheduleContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<Bus> Buses { get; set; } = null!;
        public DbSet<Stop> Stops { get; set; } = null!;
        public DbSet<Entities.Route> Routes{ get; set; } = null!;
        public DbSet<StopOrder> StopOrders { get; set; } = null!;
        public DbSet<TimeTable> TimeTables { get; set; } = null!;
        public BusScheduleContext(DbContextOptions<BusScheduleContext> options) : base(options)
        { }


    }
}
