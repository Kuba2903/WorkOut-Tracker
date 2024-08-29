using AutoMapper;
using Data;
using Data.DTO_s;
using Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExerciseController : ControllerBase
    {
        private readonly AppDbContext dbContext;
        private readonly IMapper mapper;
        public ExerciseController(AppDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        [HttpPost]
        [Route("createExercise")]

        public async Task<IActionResult> Create(ExerciseDTO dto)
        {
            var entity = mapper.Map<Exercise>(dto);

            entity.Weight = CheckNulls(dto.Weight);
            entity.Sets = CheckNulls(dto.Sets);
            entity.Repetition = CheckNulls(dto.Repetition);

            if (dto.Description.IsNullOrEmpty())
                entity.Description = null;


            var category = await dbContext.Categories.FirstOrDefaultAsync(x => x.Name == dto.Category);

            if (category == null)
            {
                var newCategory = new Category()
                {
                    Name = dto.Category
                };
                await dbContext.Categories.AddAsync(newCategory);
                await dbContext.SaveChangesAsync();
                entity.CategoryId = newCategory.Id;
            }
            else
            {
                entity.CategoryId = category.Id;
            }

            await dbContext.Exercises.AddAsync(entity);
            await dbContext.SaveChangesAsync();

            return Ok();
        }


        private static int? CheckNulls(int? x)
        {
            if (x == 0)
            {
                return null;
            }
            else
            {
                return x;
            }
        }


        [HttpPut]
        [Route("exerciseUpdate")]
        public async Task<IActionResult> Update(ExerciseDTO dto)
        {
            var entity = await dbContext.Exercises.FirstOrDefaultAsync(x => x.Name == dto.Name);
            var category = await dbContext.Categories.FirstOrDefaultAsync(x => x.Name == dto.Category);

            if(entity != null)
            {
                entity.Weight = dto.Weight;
                entity.Description = dto.Description;
                entity.Sets = dto.Sets;
                entity.Repetition = dto.Repetition;
                entity.CategoryId = category?.Id;

                await dbContext.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return NotFound("Entity not found");
            }
        }
    }
}
