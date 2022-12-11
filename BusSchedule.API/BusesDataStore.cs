using BusSchedule.API.Models;

namespace BusSchedule.API
{
    public class BusesDataStore
    {
        public static BusesDataStore Instance { get; set; } = new BusesDataStore();
        public List<BusDto> Buses { get; set; }

        public BusesDataStore()
        {
            Buses = new List<BusDto>()
            { 
                new BusDto() { Id = 0 , Name = "Bus0-2", 
                    StopsRoute = StopsDataStore.Instance.Stops.GetRange(0,3) },
                new BusDto() { Id = 1 , Name = "Bus2-4",
                    StopsRoute = StopsDataStore.Instance.Stops.GetRange(2,3) },
                new BusDto() { Id = 2 , Name = "Bus4-6",
                    StopsRoute = StopsDataStore.Instance.Stops.GetRange(4,3) },
                new BusDto() { Id = 3 , Name = "Bus6-8",
                    StopsRoute = StopsDataStore.Instance.Stops.GetRange(6,3) },
                new BusDto() { Id = 4 , Name = "Bus8-10",
                    StopsRoute = StopsDataStore.Instance.Stops.GetRange(8,3) }

            };
        }
    }
}
