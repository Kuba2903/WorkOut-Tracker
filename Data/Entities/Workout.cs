using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Workout
    {
        public int Id { get; set; }

        public List<Exercise>? Exercises { get; set; }

        public string? Comments { get; set; }

        public DateTime? ScheduleTime { get; set; }
    }
}
