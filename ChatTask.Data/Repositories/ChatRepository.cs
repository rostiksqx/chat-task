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
        var chatsEntity = await _dbContext.Chats.AsNoTracking().ToListAsync();
        
        var chats = chatsEntity.Select(chatEntity => new Chat
        {
            Id = chatEntity.Id,
            Name = chatEntity.Name,
            CreatorId = chatEntity.CreatorId,
            CreatedAt = chatEntity.CreatedAt
        }).ToList();
        
        return chats;
    }

    public async Task<Guid> Add(Chat chat)
    {
        var chatEntity = new ChatEntity
        {
            Id = chat.Id,
            Name = chat.Name,
            CreatorId = chat.CreatorId,
            CreatedAt = chat.CreatedAt
        };
        
        await _dbContext.Chats.AddAsync(chatEntity);
        await _dbContext.SaveChangesAsync();
        
        return chatEntity.Id;
    }

    public async Task<Guid> Delete(Guid chatId)
    {
        await _dbContext.Chats
            .Where(c => c.Id == chatId)
            .ExecuteDeleteAsync();
        await _dbContext.SaveChangesAsync();

        return chatId;
    }

    public async Task<Chat?> GetChatById(Guid chatId)
    {
        var chatEntity = await _dbContext.Chats
            .AsNoTracking()
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
            CreatedAt = chatEntity.CreatedAt
        };

        return chat;
    }

    public async Task<Chat?> GetChatByName(string chatName)
    {
        var chatEntity = await _dbContext.Chats
            .AsNoTracking()
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
            CreatedAt = chatEntity.CreatedAt
        };
        
        return chat;
    }
    
    public async Task<List<Chat>> GetChatsByUserId(Guid userId)
    {
        var chatsEntity = await _dbContext.Chats
            .AsNoTracking()
            .Where(c => c.CreatorId == userId)
            .ToListAsync();
        
        var chats = chatsEntity.Select(chatEntity => new Chat
        {
            Id = chatEntity.Id,
            Name = chatEntity.Name,
            CreatorId = chatEntity.CreatorId,
            CreatedAt = chatEntity.CreatedAt
        }).ToList();
        
        return chats;
    }
}