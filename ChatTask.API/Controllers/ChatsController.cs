using System.Text.RegularExpressions;
using ChatTask.API.Hubs;
using ChatTask.BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ChatTask.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatsController(IChatService chatService, IHubContext<ChatHub> hubContext)
        {
            _chatService = chatService;
            _hubContext = hubContext;
        }
        
        [HttpGet("all")]
        public async Task<IActionResult> GetAllChats()
        {
            var chats = await _chatService.GetAllChats();
            return Ok(chats);
        }
        
        [HttpGet("my-rooms")]
        public async Task<IActionResult> GetMyChats(Guid userId)
        {
            var chats = await _chatService.GetChatsByUserId(userId);
            return Ok(chats);
        }
    }
}
