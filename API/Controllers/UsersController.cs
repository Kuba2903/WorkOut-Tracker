using Data.DTO_s;
using Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserLog user;

        public UsersController(IUserLog user)
        {
            this.user = user;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            var message = await user.RegisterAsync(registerDTO);

            return Ok(message);
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var token = await user.LoginAsync(loginDTO);

            return Ok(token);
        }

    }
}
