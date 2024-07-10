using ChatTask.API.Models;
using ChatTask.BLL.Services;
using Microsoft.AspNetCore.SignalR;

namespace ChatTask.API.Hubs;

public class ChatHub : Hub
{
    private readonly IChatService _chatService;

    public ChatHub(IChatService chatService)
    {
        _chatService = chatService;
    }
    
    public async Task SendMessage(string chatRoom, string user, string message)
    {
        await Clients.Group(chatRoom).SendAsync("ReceiveMessage", user, message);
    }
    
    [HubMethodName("CreateChatRoom")]
    public async Task<Guid> CreateChatRoom(string chatRoom, Guid userId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, chatRoom);
        await Clients.All.SendAsync("ReceiveMessage", "Admin", $"Chat room {chatRoom} has been created.");
        return await _chatService.CreateChat(chatRoom, userId);
    }
    
    public async Task JoinChatRoom(string chatRoom, string username)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, chatRoom);
        await Clients.Group(chatRoom).SendAsync("ReceiveMessage", "Admin", $"{username} has joined the chat");
    }

    public async Task LeaveChatRoom(string chatRoom, Guid userId, string username)
    {
        var chat = await _chatService.GetChatByName(chatRoom);
        if (chat.CreatorId == userId)
        {
            await _chatService.DeleteChat(chat.Id, userId);
            await Clients.All.SendAsync("ReceiveMessage", "Admin", $"The chat `{chatRoom}` has been deleted by the creator.");
        }
        else
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatRoom);
            await Clients.Group(chatRoom).SendAsync("ReceiveMessage", "Admin", $"{username} has left the chat");
        }
    }
}