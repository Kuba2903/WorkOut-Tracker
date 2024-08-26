using Data;
using Data.DTO_s;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExerciseController : ControllerBase
    {
        private readonly AppDbContext dbContext;
        
        public ExerciseController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpPost]
        [Route("createExercise")]

        public async Task<IActionResult> Create(ExerciseDTO dto)
        {
            

            return Ok();
        }
    }
}
