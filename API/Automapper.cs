using AutoMapper;
using Data;
using Data.DTO_s;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
namespace API
{
    public class Automapper : Profile 
    {

        public Automapper()
        {
            CreateMap<ExerciseDTO, Exercise>()
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.CategoryId, opt => opt.Ignore())
            .ForMember(dest => dest.WorkoutExercises, opt => opt.Ignore());
        }

    }
}
