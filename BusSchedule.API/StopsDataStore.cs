using BusSchedule.API.Models;

namespace BusSchedule.API
{
    public class StopsDataStore
    {
        public List<StopDto> Stops { get; set; }

        public static StopsDataStore Current { get; set; } = new StopsDataStore();

        public StopsDataStore()
        {
            Stops = new List<StopDto>()
            {
                new StopDto()
                {
                    Id= 0,
                    Name = "Stop 0",
                    BusList = new List<BusDto>()
                    {
                        new BusDto() { Id = 0, Name = "Bus 0"},
                        new BusDto() { Id = 1, Name = "Bus 1"}
                    }
                },
                new StopDto()
                {
                    Id= 1,
                    Name = "Stop 1",
                    BusList = new List<BusDto>()
                    {
                        new BusDto() { Id = 1, Name = "Bus 1"},
                        new BusDto() { Id = 2, Name = "Bus 2"}
                    }

                },
                new StopDto()
                {
                    Id= 2,
                    Name = "Stop 2",
                    BusList = new List<BusDto>()
                    {
                        new BusDto() { Id = 0, Name = "Bus 0"},
                        new BusDto() { Id = 2, Name = "Bus 2"}
                    }
                }
            };


        }
    }
}
