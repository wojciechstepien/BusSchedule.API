using BusSchedule.API.Models;

namespace BusSchedule.API
{
    public class StopsDataStore
    {
        public static StopsDataStore Instance { get; private set; } = new StopsDataStore();
        public List<StopDto> Stops { get; set; }

        public StopsDataStore()
        {
            Stops = new List<StopDto>()
            {
                new StopDto() {Id = 0, Name = "Stop0" },
                new StopDto() {Id = 1, Name = "Stop1" },
                new StopDto() {Id = 2, Name = "Stop2" },
                new StopDto() {Id = 3, Name = "Stop3" },
                new StopDto() {Id = 4, Name = "Stop4" },
                new StopDto() {Id = 5, Name = "Stop5" },
                new StopDto() {Id = 6, Name = "Stop6" },
                new StopDto() {Id = 7, Name = "Stop7" },
                new StopDto() {Id = 8, Name = "Stop8" },
                new StopDto() {Id = 9, Name = "Stop9" },
                new StopDto() {Id = 10,Name = "Stop10"}
            };
        }
    }
}
