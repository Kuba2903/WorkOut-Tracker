using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class WorkoutUser
    {
        public int WorkoutId { get; set; }
        public int UserId { get; set; }
        public Workout Workout { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
