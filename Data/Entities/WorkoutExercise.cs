using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class WorkoutExercise
    {
        public int ExerciseId { get; set; }
        public int WorkoutId { get; set; }

        public Exercise Exercise { get; set; } = null!;
        public Workout Workout { get; set; } = null!;
    }
}
