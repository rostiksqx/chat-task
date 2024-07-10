using ChatTask.API.Models;
using Microsoft.AspNetCore.SignalR;

namespace ChatTask.API.Hubs;

public class ChatHub : Hub
{
    public async Task JoinChat(UserConnection connection)
    {
        await Clients.All
            .SendAsync("ReceiveMessage", "Admin" ,$"{connection.Username} has joined the chat");
    }
    
    public async Task JoinSpecificChatRoom(UserConnection connection)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, connection.ChatRoom);
        
        await Clients.Group(connection.ChatRoom)
            .SendAsync("ReceiveMessage", "Admin", $"{connection.Username} has joined the chat");
    }
}