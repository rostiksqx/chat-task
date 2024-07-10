using ChatTask.BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatTask.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatsController(IChatService chatService)
        {
            _chatService = chatService;
        }
        
        [HttpGet("all")]
        public async Task<IActionResult> GetAllChats()
        {
            var chats = await _chatService.GetAllChats();
            return Ok(chats);
        }
    }
}
