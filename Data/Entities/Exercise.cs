using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Exercise
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public int? Repetition { get; set; }
        public int? Sets { get; set; }
        public int? Weight { get; set; }

        public int? CategoryId { get; set; }
        [JsonIgnore]
        public Category? Category { get; set; }

        public ICollection<WorkoutExercise>? WorkoutExercises { get; set; }

    }
}
