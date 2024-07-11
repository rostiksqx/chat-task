using ChatTask.BLL.Models;

namespace ChatTask.Data.Repositories;

public interface IMessageRepository
{
    Task<List<Message>> GetMessagesByChatId(Guid chatId);
    Task<(Guid, Exception?)> Add(Message message);
    Task<(Guid, Exception?)> Delete(Guid messageId);
}