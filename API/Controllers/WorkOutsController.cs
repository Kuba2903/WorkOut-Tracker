using Data;
using Data.DTO_s;
using Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

        [HttpGet]
        [Route("getWorkout")]

        public async Task<IActionResult> Select()
        {
            var entity = await db.WorkoutExercise
                .Include(x => x.Workout)
                .Include(x => x.Exercise)
                .GroupBy(x => new
                {
                    x.Workout.Name,
                    x.Workout.Comments,
                    x.Workout.ScheduleTime
                })
                .Select(g => new
                {
                    WorkoutName = g.Key.Name,
                    Comments = g.Key.Comments,
                    ScheduleTime = g.Key.ScheduleTime,
                    Exercises = g.Select(e => new
                    {
                        ExerciseName = e.Exercise.Name,
                        ExerciseDescription = e.Exercise.Description
                    }).ToList()
                })
                .ToListAsync();

            return Ok(entity);
        }

        [HttpPost]
        [Route("createWorkout")]
        public async Task<IActionResult> Create(WorkOutDTO dto)
        {
            var exercises = await db.Exercises
                        .Where(e => dto.ExerciseNames.Contains(e.Name))
                        .ToListAsync();


            Workout workout = new Workout()
            {
                Name = dto.Name,
                Comments = dto.Comment,
                ScheduleTime = dto.ScheduleTime
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

        [HttpPut]
        [Route("updateWorkout")]
        public async Task<IActionResult> Update(WorkOutDTO dto)
        {
            var workout = await db.Workouts.FirstOrDefaultAsync(x => x.Name == dto.Name);
            var exercises = await db.Exercises.Where(e => dto.ExerciseNames.Contains(e.Name)).ToListAsync();
            var workoutExercise = await db.WorkoutExercise.Where(x => x.WorkoutId == workout.Id).ToListAsync();

            List<WorkoutExercise> modified = new List<WorkoutExercise>();
            workout.Comments += $". {dto.Comment}";

            foreach (var exercise in exercises)
            {
                foreach (var workoutexercsise in workoutExercise)
                {
                    if(workoutexercsise.ExerciseId != exercise.Id)
                    {
                        modified.Add(new WorkoutExercise { ExerciseId = exercise.Id, WorkoutId = workout.Id });
                    }
                }
            }

            foreach (var item in modified)
            {
                await db.WorkoutExercise.AddAsync(item);
            }

            await db.SaveChangesAsync();

            return Ok("Updated");
        }



        [HttpDelete]
        [Route("deleteWorkout")]

        public async Task<IActionResult> Delete(string name)
        {
            var entity = await db.Workouts.FirstOrDefaultAsync(x => x.Name == name);

            if(entity != null)
            {
                db.Workouts.Remove(entity);
                await db.SaveChangesAsync();
                return Ok("Workout removed");
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Allows to schedule workouts for specific date
        /// </summary>
        /// <param name="name"></param>
        /// <param name="timeSchedule"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("schedule")]
        public async Task<IActionResult> Schedule(string name, DateTime timeSchedule)
        {
            var entity = await db.Workouts.Where(x => !x.ScheduleTime.HasValue).FirstOrDefaultAsync(x => x.Name == name);
        
            entity.ScheduleTime = timeSchedule;

            await db.SaveChangesAsync();

            return Ok($"Workout {entity.Name} scheduled on {entity.ScheduleTime}");
        }
    }
}
