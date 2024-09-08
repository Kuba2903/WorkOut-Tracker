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


        /// <summary>
        /// Allows to add more exercises for the workout
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>

        [HttpPut]
        [Route("updateWorkout")]
        public async Task<IActionResult> Update(WorkOutDTO dto)
        {

            var workoutExercise = await db.WorkoutExercise.Include(x => x.Workout)
                .FirstOrDefaultAsync(x => x.Workout.Name == dto.Name);

            workoutExercise.Workout.Comments += $". {dto.Comment}";

            var exercises = await db.Exercises.Where(e => dto.ExerciseNames.Contains(e.Name)).ToListAsync();


            foreach (var x in exercises)
            {
                if(workoutExercise.ExerciseId != x.Id)
                {
                    WorkoutExercise entity = new WorkoutExercise
                    {
                        ExerciseId = x.Id,
                        WorkoutId = workoutExercise.WorkoutId
                    };
                    await db.WorkoutExercise.AddAsync(entity);
                }
            }

            await db.SaveChangesAsync();

            return Ok("Updated");
        }

        /// <summary>
        /// Delete workout from the database
        /// </summary>
        /// <param name="name">enter the name of workout you want to delete</param>
        /// <returns></returns>

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
        /// <param name="name">enter the name of workout you want to schedule</param>
        /// <param name="timeSchedule">schedule with the provided date</param>
        /// <returns></returns>
        [HttpPut]
        [Route("schedule")]
        public async Task<IActionResult> Schedule(string name, DateTime timeSchedule)
        {
            var entity = await db.Workouts.Where(x => !x.ScheduleTime.HasValue).
                FirstOrDefaultAsync(x => x.Name == name);
        
            entity.ScheduleTime = timeSchedule;

            await db.SaveChangesAsync();

            return Ok($"Workout {entity.Name} scheduled on {entity.ScheduleTime}");
        }

        /// <summary>
        /// Generates raport on past workouts
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [Route("raport")]

        public async Task<IActionResult> GenerateRaport()
        {
            var entity = await db.WorkoutExercise
                .Include(x => x.Workout)
                .Include(x => x.Exercise).ThenInclude(x => x.Category).
                    Where(y => y.Workout.ScheduleTime < DateTime.Today)
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
                        ExerciseDescription = e.Exercise.Description,
                        Repetition = e.Exercise.Repetition,
                        Sets = e.Exercise.Sets,
                        Weight = e.Exercise.Weight,
                        Category = e.Exercise.Category.Name
                    }).ToList()
                })
                .ToListAsync();

            return Ok(entity);
        }
    }
}
