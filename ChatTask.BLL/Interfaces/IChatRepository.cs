using ChatTask.BLL.Models;

namespace ChatTask.Data.Repositories;

public interface IChatRepository
{
    Task<List<Chat>> GetAllChats();
    Task<Guid> Add(Chat chat);
    Task<Guid> Delete(Guid chatId);
    Task<Chat?> GetChatById(Guid chatId);
    Task<Chat?> GetChatByName(string chatName);
    Task<List<Chat>> GetChatsByUserId(Guid userId);
}