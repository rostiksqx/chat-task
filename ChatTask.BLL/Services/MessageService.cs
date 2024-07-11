using ChatTask.BLL.Models;
using ChatTask.Data.Repositories;

namespace ChatTask.BLL.Services;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _messageRepository;

    public MessageService(IMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }
    
    public async Task<List<Message>> GetMessagesByChatId(Guid chatId)
    {
        return await _messageRepository.GetMessagesByChatId(chatId);
    }
    
    public async Task<Guid> Add(Guid chatId, Guid userId, string message)
    {
        var newMessage = new Message
        {
            Id = Guid.NewGuid(),
            ChatId = chatId,
            SenderId = userId,
            Text = message
        };
        
        return await _messageRepository.Add(newMessage);
    }
    
    public async Task<Guid> Delete(Guid messageId)
    {
        return await _messageRepository.Delete(messageId);
    }
}