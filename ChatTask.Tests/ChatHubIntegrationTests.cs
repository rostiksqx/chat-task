using System;
using System.Net.Http;
using System.Threading.Tasks;
using ChatTask.API;
using ChatTask.API.Hubs;
using ChatTask.BLL.Models;
using ChatTask.BLL.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace ChatTask.Tests;

public class ChatHubIntegrationTests
{
    private readonly TestServer _server;
    private readonly HttpClient _client;
    private readonly HubConnection _hubConnection;
    private readonly Mock<IChatService> _mockChatService;
    private readonly Mock<IMessageService> _mockMessageService;

    public ChatHubIntegrationTests()
    {
        _mockChatService = new Mock<IChatService>();
        _mockMessageService = new Mock<IMessageService>();

        // Настройка TestServer
        var webHostBuilder = new WebHostBuilder()
            .UseUrls("https://localhost:7128")
            .ConfigureServices(services =>
            {
                services.AddSingleton<IChatService>(_mockChatService.Object);
                services.AddSingleton<IMessageService>(_mockMessageService.Object);
                services.AddSignalR();
            })
            .Configure(app =>
            {
                app.UseRouting();
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapHub<ChatHub>("/chat-hub");
                });
            });

        _server = new TestServer(webHostBuilder);

        _client = _server.CreateClient();

        // Настройка HubConnection
        _hubConnection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7128/chat-hub", options =>
            {
                options.HttpMessageHandlerFactory = _ => _server.CreateHandler();
            })
            .Build();
    }

    [Fact]
    public async Task SendMessage_ChatRoomDoesNotExist_ShouldReturnErrorMessage()
    {
        // Arrange
        var chatRoom = "TestRoom";
        var userId = Guid.NewGuid();
        var message = "TestMessage";
        _mockChatService.Setup(s => s.GetChatByName("TestRoom")).ReturnsAsync((Chat)null);

        var tcs = new TaskCompletionSource<string>();
        var receivedMessage = string.Empty;
        _hubConnection.On<string, string>("ReceiveMessage", (sender, received) =>
        {
            if (sender == "Admin")
            {
                receivedMessage = received;
                tcs.SetResult(received); // Signal that the message has been received
            }
        });

        await _hubConnection.StartAsync();

        // Act
        await _hubConnection.InvokeAsync("SendMessage", chatRoom, userId, message);

        // Wait for the message to be received
        await tcs.Task;

        // Assert
        Assert.Equal($"Chat room `{chatRoom}` does not exist. Check the chat room name and try again.", receivedMessage);

        await _hubConnection.StopAsync();
    }

    [Fact]
    public async Task SendMessage_ChatRoomExists_ShouldSendMessageToGroup()
    {
        // Arrange
        var chatRoom = "TestRoom";
        var userId = Guid.NewGuid();
        var message = "TestMessage";
        var chat = new Chat { Id = Guid.NewGuid(), Name = chatRoom };
        _mockChatService.Setup(s => s.GetChatByName(chatRoom)).ReturnsAsync(chat);
    
        // Simulate joining the chat room
        _mockChatService.Setup(s => s.GetChatByName(chatRoom)).ReturnsAsync(new Chat { Id = Guid.NewGuid(), Name = chatRoom });
    
        var tcs = new TaskCompletionSource<string>();
        var receivedMessage = string.Empty;
        _hubConnection.On<Guid, string>("ReceiveMessage", (senderId, received) =>
        {
            if (senderId == userId)
            {
                receivedMessage = received;
                tcs.SetResult(received); // Signal that the message has been received
            }
        });
    
        await _hubConnection.StartAsync();
    
        // Act
        // First, join the chat room
        await _hubConnection.InvokeAsync("JoinChatRoom", chatRoom, "TestUser");
        // Then, send the message
        await _hubConnection.InvokeAsync("SendMessage", chatRoom, userId, message);
    
        // Wait for the message to be received
        await tcs.Task;
    
        // Assert
        Assert.Equal(message, receivedMessage);
    
        await _hubConnection.StopAsync();
    }
}