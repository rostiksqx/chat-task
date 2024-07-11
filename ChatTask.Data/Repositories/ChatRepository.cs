using ChatTask.BLL.Models;
using ChatTask.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatTask.Data.Repositories;

public class ChatRepository : IChatRepository
{
    private readonly ChatDbContext _dbContext;

    public ChatRepository(ChatDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<List<Chat>> GetAllChats()
    {
        return await _dbContext.Chats
            .AsNoTracking()
            .Include(c => c.Messages)
            .Select(chatEntity => new Chat
            {
                Id = chatEntity.Id,
                Name = chatEntity.Name,
                CreatorId = chatEntity.CreatorId,
                Messages = chatEntity.Messages.Select(m => new Message
                {
                    Id = m.Id,
                    ChatId = m.ChatId,
                    SenderId = m.SenderId,
                    Text = m.Text
                }).ToList(),
                CreatedAt = chatEntity.CreatedAt
            }).ToListAsync();
    }

    public async Task<(Guid, Exception?)> Add(Chat chat)
    {
        var chatEntity = new ChatEntity
        {
            Id = chat.Id,
            Name = chat.Name,
            CreatorId = chat.CreatorId,
            CreatedAt = chat.CreatedAt
        };
        
        await _dbContext.Chats.AddAsync(chatEntity);
        var ex = await _dbContext.SafeSave();

        if (ex != null)
        {
            return (Guid.Empty, ex);
        }
        
        return (chatEntity.Id, null);
    }

    public async Task<(Guid, Exception?)> Delete(Guid chatId)
    {
        await _dbContext.Chats
            .Where(c => c.Id == chatId)
            .ExecuteDeleteAsync();

        var ex = await _dbContext.SafeSave();

        if (ex != null)
        {
            return (Guid.Empty, ex);
        }
        
        return (chatId, null);
    }

    public async Task<Chat?> GetChatById(Guid chatId)
    {
        var chatEntity = await _dbContext.Chats
            .AsNoTracking()
            .Include(c => c.Messages)
            .FirstOrDefaultAsync(c => c.Id == chatId);

        if (chatEntity == null)
        {
            return null;
        }
        
        var chat = new Chat
        {
            Id = chatEntity.Id,
            Name = chatEntity.Name,
            CreatorId = chatEntity.CreatorId,
            Messages = chatEntity.Messages.Select(m => new Message
            {
                Id = m.Id,
                ChatId = m.ChatId,
                SenderId = m.SenderId,
                Text = m.Text
            }).ToList(),
            CreatedAt = chatEntity.CreatedAt
        };

        return chat;
    }

    public async Task<Chat?> GetChatByName(string chatName)
    {
        var chatEntity = await _dbContext.Chats
            .AsNoTracking()
            .Include(c => c.Messages)
            .FirstOrDefaultAsync(c => c.Name == chatName);

        if (chatEntity == null)
        {
            return null;
        }

        var chat = new Chat
        {
            Id = chatEntity.Id,
            Name = chatEntity.Name,
            CreatorId = chatEntity.CreatorId,
            Messages = chatEntity.Messages.Select(m => new Message
            {
                Id = m.Id,
                ChatId = m.ChatId,
                SenderId = m.SenderId,
                Text = m.Text
            }).ToList(),
            CreatedAt = chatEntity.CreatedAt
        };
        
        return chat;
    }
    
    public async Task<List<Chat>> GetChatsByUserId(Guid userId)
    {
        var chatsEntity = await _dbContext.Chats
            .AsNoTracking()
            .Include(c => c.Messages)
            .Where(c => c.CreatorId == userId)
            .ToListAsync();
        
        var chats = chatsEntity.Select(chatEntity => new Chat
        {
            Id = chatEntity.Id,
            Name = chatEntity.Name,
            CreatorId = chatEntity.CreatorId,
            Messages = chatEntity.Messages.Select(m => new Message
            {
                Id = m.Id,
                ChatId = m.ChatId,
                SenderId = m.SenderId,
                Text = m.Text
            }).ToList(),
            CreatedAt = chatEntity.CreatedAt
        }).ToList();
        
        return chats;
    }
    
    public async Task<(Guid, Exception?)> Update(Guid chatId, string newChatName)
    {
        var chatEntity = await _dbContext.Chats
            .FirstOrDefaultAsync(c => c.Id == chatId);

        if (chatEntity == null)
        {
            throw new ArgumentException("Chat not found");
        }

        chatEntity.Name = newChatName;

        var ex = await _dbContext.SafeSave();

        if (ex != null)
        {
            return (Guid.Empty, ex);
        }
        
        return (chatId, null);
    }
}