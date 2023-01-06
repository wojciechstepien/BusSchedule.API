using BusSchedule.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusSchedule.API.Entities
{
    public class TimeTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Bus? Bus { get; set; }
        public Stop? Stop { get; set; }
        public TimeOnly? Time { get; set; }
    }
}
