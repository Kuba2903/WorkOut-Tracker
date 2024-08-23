using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Exercise
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }


        public string? Category { get; set; }

        public int? Repetition { get; set; }
        public int? Sets { get; set; }
        public int? Weight { get; set; }

        public ICollection<WorkoutExercise>? WorkoutExercises { get; set; }

    }
}
