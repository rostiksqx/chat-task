using ChatTask.BLL.Models;

namespace ChatTask.BLL.Services;

public interface IChatService
{
    Task<List<Chat>> GetAllChats();
    Task<(Guid, Exception?)> CreateChat(string name, Guid creatorId);
    Task<(Guid, Exception?)> DeleteChat(Guid chatId, Guid creatorId);
    Task<Chat?> GetChatByName(string chatName);
    Task<List<Chat>?> GetChatsByUserId(Guid userId);
    Task<(Guid, Exception?)> UpdateChat(Guid chatId, string newChatName);
    Task<Chat?> GetChatById(Guid chatId);
}