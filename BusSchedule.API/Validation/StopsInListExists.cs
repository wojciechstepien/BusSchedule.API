using BusSchedule.API.Models;
using System.ComponentModel.DataAnnotations;

namespace BusSchedule.API.Validation
{
    public class StopsInListExists : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var list = value as List<StopDto>;

            if (list == null) { return false; }
            foreach (StopDto stop in list)
            {
                var exists = false;
                foreach (StopDto stopDto in StopsDataStore.Instance.Stops)
                {
                    if(stopDto.Id == stop.Id & stopDto.Name == stop.Name)
                    {
                        exists = true;
                    }
                }
                if(!exists) { return false; }
            }
            return true;
        }
    }
}
