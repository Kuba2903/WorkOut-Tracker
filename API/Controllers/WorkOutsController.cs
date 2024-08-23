using Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> CreateWorkout()
        {

            return Ok();
        }
    }
}
