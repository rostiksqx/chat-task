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
        public async Task<Guid> RegisterUser(UserRequest user)
        {
            return await _userService.Register(user.Username, user.Password);
        }

        [HttpPost("login")]
        public async Task<Guid> Login(UserRequest user)
        {
            return await _userService.Login(user.Username, user.Password);
        }
    }
}
