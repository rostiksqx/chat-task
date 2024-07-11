using ChatTask.BLL.Models;

namespace ChatTask.Data.Repositories;

public interface IMessageRepository
{
    Task<List<Message>> GetMessagesByChatId(Guid chatId);
    Task<Guid> Add(Message message);
    Task<Guid> Delete(Guid messageId);
}