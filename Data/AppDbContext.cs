﻿using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Exercise> Exercises { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Workout> Workouts { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<WorkoutExercise> WorkoutExercise { get; set; }
        public DbSet<WorkoutUser> WorkoutUser { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseSqlServer("Data Source=HP;Initial Catalog=ExerciseDb;Integrated Security=True;Trust Server Certificate=True");
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WorkoutExercise>()
                .HasKey(x => new { x.ExerciseId, x.WorkoutId });

            modelBuilder.Entity<WorkoutUser>()
                .HasKey(x => new { x.WorkoutId, x.UserId });
        }
    }
}
