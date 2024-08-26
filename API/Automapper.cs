using AutoMapper;
using Data.DTO_s;
using Data.Entities;
namespace API
{
    public class Automapper : Profile 
    {
        public Automapper()
        {
            CreateMap<ExerciseDTO, Exercise>();
        }
    }
}
