using ChatTask.BLL.Models;

namespace ChatTask.BLL.Services;

public interface IChatService
{
    Task<List<Chat>> GetAllChats();
    Task<Guid> CreateChat(string name, Guid creatorId);
    Task<Guid> DeleteChat(Guid chatId, Guid creatorId);
    Task<Chat?> GetChatByName(string chatName);
    Task<List<Chat>?> GetChatsByUserId(Guid userId);
    Task<Chat> UpdateChat(Guid chatId, string newChatName);
}