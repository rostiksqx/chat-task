using ChatTask.BLL.Models;

namespace ChatTask.Data.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserByUserName(string username);
    Task<Guid> Add(User user);
    
    Task<User?> GetUserById(Guid id);
}