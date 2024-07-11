using System;
using System.Threading.Tasks;
using ChatTask.API.Hubs;
using ChatTask.BLL.Models;
using ChatTask.BLL.Services;
using Microsoft.AspNetCore.SignalR;
using Moq;
using Xunit;

namespace ChatTask.Tests;


public class ChatHubTests
{
    private readonly Mock<IChatService> _mockChatService;
    private readonly Mock<IMessageService> _mockMessageService;
    private readonly ChatHub _chatHub;
    private readonly Mock<HubCallerContext> _mockContext;
    private readonly Mock<IGroupManager> _mockGroups;
    private readonly Mock<ISingleClientProxy> _mockClientsProxy;
    private readonly Mock<IHubCallerClients> _mockClients;

    public ChatHubTests()
    {
        _mockChatService = new Mock<IChatService>();
        _mockMessageService = new Mock<IMessageService>();
        _mockContext = new Mock<HubCallerContext>();
        _mockGroups = new Mock<IGroupManager>();
        _mockClientsProxy = new Mock<ISingleClientProxy>();
        _mockClients = new Mock<IHubCallerClients>();

        _chatHub = new ChatHub(_mockChatService.Object, _mockMessageService.Object)
        {
            Context = _mockContext.Object,
            Groups = _mockGroups.Object,
            Clients = _mockClients.Object
        };
    }
    
    [Fact]
    public async Task SendMessage_ChatRoomDoesNotExist_ShouldReturnErrorMessage()
    {
        // Arrange
        var chatRoom = "TestRoom";
        var userId = Guid.NewGuid();
        var message = "TestMessage";
        _mockChatService.Setup(s => s.GetChatByName(It.IsAny<string>())).ReturnsAsync((Chat)null);
        _mockClients.Setup(c => c.Caller).Returns(_mockClientsProxy.Object);

        // Act
        await _chatHub.SendMessage(chatRoom, userId, message);

        // Assert
        _mockClientsProxy.Verify(c => c.SendCoreAsync("ReceiveMessage", It.Is<object[]>(o => 
                o.Length == 2 && (string)o[0] == "Admin" && (string)o[1] == $"Chat room `{chatRoom}` does not exist. Check the chat room name and try again."), 
            default), Times.Once);
    }

    [Fact]
    public async Task SendMessage_ChatRoomExists_ShouldSendMessageToGroup()
    {
        // Arrange
        var chatRoom = "TestRoom";
        var userId = Guid.NewGuid();
        var message = "TestMessage";
        var chat = new Chat { Id = Guid.NewGuid(), Name = chatRoom };
        _mockChatService.Setup(s => s.GetChatByName(It.IsAny<string>())).ReturnsAsync(chat);
        _mockClients.Setup(c => c.Group(It.IsAny<string>())).Returns(_mockClientsProxy.Object);

        // Act
        await _chatHub.SendMessage(chatRoom, userId, message);

        // Assert
        _mockClientsProxy.Verify(c => c.SendCoreAsync("ReceiveMessage", It.Is<object[]>(o =>
                o.Length == 2 && (Guid)o[0] == userId && (string)o[1] == message), 
            default), Times.Once);
    }
}