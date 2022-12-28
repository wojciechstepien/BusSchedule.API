using BusSchedule.API.Models;
using System.ComponentModel.DataAnnotations;

namespace BusSchedule.API.Validation
{
    public class StopExists : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            StopDto? stop = value as StopDto;
            if (stop == null) { return false; }
            var exists = false;
            foreach (StopDto stopDto in StopsDataStore.Instance.Stops)
            {
                if (stopDto.Id == stop.Id & stopDto.Name == stop.Name)
                {
                    exists = true;
                }
            }
            if (!exists) { return false; }
            return true;
        }
    }
}
