using ChatTask.API.Models;
using ChatTask.BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatTask.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        
        [HttpPost("register")]
        public async Task<ActionResult<Guid>> Register(UserRequest user)
        {
            return Ok(await _userService.Register(user.Username, user.Password));
        }

        [HttpPost("login")]
        public async Task<ActionResult<Guid>> Login(UserRequest user)
        {
            return Ok(await _userService.Login(user.Username, user.Password));
        }
    }
}
