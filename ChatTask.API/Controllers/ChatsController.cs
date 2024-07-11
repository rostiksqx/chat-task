using System.Text.RegularExpressions;
using ChatTask.API.Hubs;
using ChatTask.API.Models;
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

        public ChatsController(IChatService chatService)
        {
            _chatService = chatService;
        }
        
        [HttpGet("all")]
        public async Task<ActionResult<ChatResponse>> GetAllChats()
        {
            var chats = await _chatService.GetAllChats();
            
            var chatResponses = chats.Select(chat => new ChatResponse
            {
                Id = chat.Id,
                Name = chat.Name,
                Messages = chat.Messages,
                CreatedAt = chat.CreatedAt
            }).ToList();
            
            return Ok(chatResponses);
        }
        
        [HttpGet("my-rooms")]
        public async Task<ActionResult<ChatResponse>> GetMyChats(Guid userId)
        {
            var chats = await _chatService.GetChatsByUserId(userId);
            return Ok(chats);
        }
        
        [HttpPost("create")]
        public async Task<ActionResult<Guid>> CreateChatRoom(string chatName, Guid userId)
        {
            var chat = await _chatService.GetChatByName(chatName);
            
            if (chat != null)
            {
                return BadRequest("Chat room with this name already exists. Please choose another name.");
            }
            
            return Ok(await _chatService.CreateChat(chatName, userId));
        }
        
        [HttpPut("update")]
        public async Task<ActionResult<ChatResponse>> UpdateChatRoom(Guid chatId, string newChatName)
        {
            var chat = await _chatService.GetChatById(chatId);
            
            if (chat == null)
            {
                return BadRequest("Chat not found");
            }
            
            return Ok(await _chatService.UpdateChat(chatId, newChatName));
        }
        
        [HttpDelete("delete")]
        public async Task<ActionResult<Guid>> DeleteChatRoom(Guid chatId, Guid userId)
        {
            try
            {
                return Ok(await _chatService.DeleteChat(chatId, userId));
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
