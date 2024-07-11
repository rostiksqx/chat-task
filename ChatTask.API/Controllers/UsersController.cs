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
            var result = await _userService.Register(user.Username, user.Password);
            
            if (result.Item2 != null)
            {
                return BadRequest(result.Item2.Message);
            }
            
            return Ok(result.Item1);
        }

        [HttpPost("login")]
        public async Task<ActionResult<Guid>> Login(UserRequest user)
        {
            return Ok(await _userService.Login(user.Username, user.Password));
        }
    }
}
