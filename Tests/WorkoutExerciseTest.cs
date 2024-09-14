using AutoMapper;
using Azure;
using Data;
using Data.DTO_s;
using Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text;

namespace Tests
{
    public class WorkoutExerciseTest : IClassFixture<AppTest<Program>>
    {
        private readonly HttpClient client;
        private readonly AppTest<Program> _appTest;

        public WorkoutExerciseTest( AppTest<Program> appTest)
        {
            client = appTest.CreateClient();
            _appTest = appTest;
        }




        private async Task SeedData(AppDbContext db)
        {
            var categories = new HashSet<Category>
            {
                new Category { Name = "Balance"},
                new Category { Name = "Cardio"},
                new Category { Name = "Strength"},
            };

            await db.Categories.AddRangeAsync(categories);
            await db.SaveChangesAsync();

            var exercises = new HashSet<Exercise>
            {
                new Exercise { Name = "exercise0", CategoryId = 2, Repetition = 5, Sets = 4, Weight = 10 },
                new Exercise { Name = "exercise1", CategoryId = 1, Repetition = 3, Sets = 6, Weight = 15
                , Description = "exercise1 description" },
                new Exercise { Name = "exercise2", CategoryId = 3, Repetition = 5, Sets = 4, Weight = 10, 
                    Description = "exercise2 description" },
                new Exercise { Name = "exercise3", CategoryId = 1, Description = "exercise3 description" }
            };

            var workouts = new HashSet<Workout>
            {
                new Workout { Name = "workout0", Comments = "workout0 comment"},
                new Workout { Name = "workout1", Comments = "workout1 comment"},
                new Workout { Name = "workout2", ScheduleTime = DateTime.Today},
            };

            await db.Exercises.AddRangeAsync(exercises);
            await db.Workouts.AddRangeAsync(workouts);
            await db.SaveChangesAsync();

            var exercise_workout = new HashSet<WorkoutExercise>
            {
                new WorkoutExercise { WorkoutId = 1, ExerciseId = 2 },
                new WorkoutExercise { WorkoutId = 1, ExerciseId = 3 },
                new WorkoutExercise { WorkoutId = 1, ExerciseId = 4 },
                new WorkoutExercise { WorkoutId = 2, ExerciseId = 1 },
                new WorkoutExercise { WorkoutId = 2, ExerciseId = 3 },
                new WorkoutExercise { WorkoutId = 2, ExerciseId = 4 },
                new WorkoutExercise { WorkoutId = 3, ExerciseId = 2 },
                new WorkoutExercise { WorkoutId = 3, ExerciseId = 1 },
            };

            await db.WorkoutExercise.AddRangeAsync(exercise_workout);
            await db.SaveChangesAsync();
        }


        private async Task ClearData(AppDbContext db)
        {
            db.WorkoutExercise.RemoveRange(db.WorkoutExercise);
            db.Workouts.RemoveRange(db.Workouts);
            db.Exercises.RemoveRange(db.Exercises);
            db.Categories.RemoveRange(db.Categories);

            await db.SaveChangesAsync();
        }


        [Fact]

        public async Task GetShouldReturnOkStatus()
        {
            using (var scope = _appTest.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await ClearData(context);
                await SeedData(context);
            }


            var test = await client.GetAsync("https://localhost:7241/api/Exercise/getExercises");

            Assert.True(test.IsSuccessStatusCode);
        }


        [Fact]

        public async Task PostShouldReturnOkStatus()
        {
            using (var scope = _appTest.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await ClearData(context);
                await SeedData(context);
            }


            ExerciseDTO entity = new ExerciseDTO()
            {
                Name = "test",
                Category = "Cardio",
                Description = "test description",
                Repetition = 5,
                Sets = 5,
                Weight = 5
            };


            var test = await client.PostAsJsonAsync("https://localhost:7241/api/Exercise/createExercise", entity);

            Assert.Equal(HttpStatusCode.OK, test.StatusCode);
        }



        [Fact]

        public async Task DeleteShouldReturnOkStatus()
        {
            using (var scope = _appTest.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await ClearData(context);
                await SeedData(context);
            }

            var test = await client.DeleteAsync("https://localhost:7241/api/Exercise/exerciseDelete?name=exercise0");

            Assert.Equal(HttpStatusCode.OK, test.StatusCode);
        }


        [Fact]

        public async Task PutShouldReturnOkStatus()
        {
            using (var scope = _appTest.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await ClearData(context);
                await SeedData(context);


                var entity = await context.Exercises.FirstAsync();

                entity.Sets = 1;
                entity.Weight = 20;
                entity.Repetition = 10;

                var test = await client.PutAsJsonAsync("https://localhost:7241/api/Exercise/exerciseUpdate", entity);
                Assert.Equal(HttpStatusCode.OK, test.StatusCode);
            }

        }
    }
}
