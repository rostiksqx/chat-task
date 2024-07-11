using ChatTask.API.Models;
using ChatTask.BLL.Services;
using Microsoft.AspNetCore.SignalR;

namespace ChatTask.API.Hubs;

public class ChatHub : Hub
{
    private readonly IChatService _chatService;
    private readonly IMessageService _messageService;

    public ChatHub(IChatService chatService, IMessageService messageService)
    {
        _chatService = chatService;
        _messageService = messageService;
    }
    
    [HubMethodName("SendMessage")]
    public async Task SendMessage(string chatRoom, Guid userId, string message)
    {
        var existChat = await _chatService.GetChatByName(chatRoom);
        
        if (existChat == null)
        {
            await Clients.Caller.SendAsync("ReceiveMessage", "Admin", $"Chat room {chatRoom} does not exist. Check the chat room name and try again.");
            return;
        }
        
        await Clients.Group(chatRoom).SendAsync("ReceiveMessage", userId, message);
        
        await _messageService.Add(existChat.Id, userId, message);
    }
    
    [HubMethodName("CreateChatRoom")]
    public async Task<Guid> CreateChatRoom(string chatRoom, Guid userId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, chatRoom);
        await Clients.All.SendAsync("ReceiveMessage", "Admin", $"Chat room {chatRoom} has been created.");
        return await _chatService.CreateChat(chatRoom, userId);
    }
    
    [HubMethodName("JoinChatRoom")]
    public async Task JoinChatRoom(string chatRoom, string username)
    {
        var chat = await _chatService.GetChatByName(chatRoom);
        
        if (chat == null)
        {
            await Clients.Caller.SendAsync("ReceiveMessage", "Admin", $"Chat room {chatRoom} does not exist. Check the chat room name and try again.");
            return;
        }
        
        await Groups.AddToGroupAsync(Context.ConnectionId, chatRoom);
        await Clients.Group(chatRoom).SendAsync("ReceiveMessage", "Admin", $"{username} has joined to the {chatRoom}");
    }

    [HubMethodName("LeaveChatRoom")]
    public async Task LeaveChatRoom(string chatRoom, Guid userId, string username)
    {
        var chat = await _chatService.GetChatByName(chatRoom);
        
        if (chat == null)
        {
            await Clients.Caller.SendAsync("ReceiveMessage", "Admin", $"Chat room {chatRoom} does not exist. Leave failed.");
            return;
        }
        
        if (chat.CreatorId == userId)
        {
            await _chatService.DeleteChat(chat.Id, userId);
            await Clients.All.SendAsync("ReceiveMessage", "Admin", $"The chat `{chatRoom}` has been deleted by the creator.");
        }
        else
        {
            await Clients.Group(chatRoom).SendAsync("ReceiveMessage", "Admin", $"{username} has left the chat");
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatRoom);
        }
    }
    
    [HubMethodName("UpdateChatRoom")]
    public async Task UpdateChatRoom(string chatRoom, Guid userId, string newChatRoom)
    {
        var chat = await _chatService.GetChatByName(chatRoom);
        
        if (chat == null)
        {
            await Clients.Caller.SendAsync("ReceiveMessage", "Admin", $"Chat room {chatRoom} does not exist. Update failed.");
            return;
        }
        
        if (chat.CreatorId == userId)
        {
            await _chatService.UpdateChat(chat.Id, newChatRoom);
            await Clients.All.SendAsync("ReceiveMessage", "Admin", $"The chat `{chatRoom}` has been updated to `{newChatRoom}` by the creator.");
        }
    }
}