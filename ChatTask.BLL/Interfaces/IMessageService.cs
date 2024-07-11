using ChatTask.BLL.Models;

namespace ChatTask.BLL.Services;

public interface IMessageService
{
    Task<List<Message>> GetMessagesByChatId(Guid chatId);
    Task<Guid> Add(Guid chatId, Guid userId, string message);
    Task<Guid> Delete(Guid messageId);
}