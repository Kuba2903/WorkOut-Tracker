using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Workout
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string? Comments { get; set; }

        public DateTime? ScheduleTime { get; set; }

        [JsonIgnore]
        public ICollection<WorkoutExercise>? WorkoutExercises { get; set; }
        public ICollection<WorkoutUser>? WorkoutUsers { get; set; }
    }
}
