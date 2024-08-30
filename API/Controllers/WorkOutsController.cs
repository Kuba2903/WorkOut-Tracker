using Data;
using Data.DTO_s;
using Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkOutsController : ControllerBase
    {
        private readonly AppDbContext db;

        public WorkOutsController(AppDbContext db)
        {
            this.db = db;
        }

        [HttpPost]
        [Route("createWorkout")]
        public async Task<IActionResult> CreateWorkout(WorkOutDTO dto)
        {
            var exercises = await db.Exercises
                        .Where(e => dto.ExerciseNames.Contains(e.Name))
                        .ToListAsync();


            Workout workout = new Workout()
            {
                Comments = dto.Comment
            };

            await db.Workouts.AddAsync(workout);
            await db.SaveChangesAsync();

            foreach (var exercise in exercises)
            {
                WorkoutExercise workoutExercise = new WorkoutExercise()
                {
                    WorkoutId = workout.Id,
                    ExerciseId = exercise.Id
                };

                await db.WorkoutExercise.AddAsync(workoutExercise);
            }

            await db.SaveChangesAsync();

            return Ok("Workout added");
        }
    }
}
