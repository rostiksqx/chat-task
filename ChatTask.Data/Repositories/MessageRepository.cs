using ChatTask.BLL.Models;
using ChatTask.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatTask.Data.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly ChatDbContext _dbContext;

    public MessageRepository(ChatDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<List<Message>> GetMessagesByChatId(Guid chatId)
    {
        return await _dbContext.Messages
            .AsNoTracking()
            .Where(m => m.ChatId == chatId)
            .Select(messageEntity => new Message
            {
                Id = messageEntity.Id,
                ChatId = messageEntity.ChatId,
                SenderId = messageEntity.SenderId,
                Text = messageEntity.Text
            }).ToListAsync();
    }
    
    public async Task<(Guid, Exception?)> Add(Message message)
    {
        var messageEntity = new MessageEntity
        {
            Id = message.Id,
            ChatId = message.ChatId,
            SenderId = message.SenderId,
            Text = message.Text
        };
        
        await _dbContext.Messages.AddAsync(messageEntity);
        var ex = await _dbContext.SafeSave();
        
        if (ex != null)
        {
            return (Guid.Empty, ex);
        }
        
        return (messageEntity.Id, null);
    }
    
    public async Task<(Guid, Exception?)> Delete(Guid messageId)
    {
        await _dbContext.Messages
            .Where(m => m.Id == messageId)
            .ExecuteDeleteAsync();

        var ex = await _dbContext.SafeSave();
        
        if (ex != null)
        {
            return (Guid.Empty, ex);
        }

        return (messageId, null);
    }
}