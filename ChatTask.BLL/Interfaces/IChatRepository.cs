using ChatTask.BLL.Models;

namespace ChatTask.Data.Repositories;

public interface IChatRepository
{
    Task<List<Chat>> GetAllChats();
    Task<(Guid, Exception?)> Add(Chat chat);
    Task<(Guid, Exception?)> Delete(Guid chatId);
    Task<Chat?> GetChatById(Guid chatId);
    Task<Chat?> GetChatByName(string chatName);
    Task<List<Chat>> GetChatsByUserId(Guid userId);
    Task<(Guid, Exception?)> Update(Guid chatId, string newChatName);
}