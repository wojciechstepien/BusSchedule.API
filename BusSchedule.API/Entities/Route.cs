using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BusSchedule.API.Entities
{
    public class Route
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Bus Bus { get; set; }
        public Stop Stop { get; set; }
        public int Order { get; set; }
    }
}
