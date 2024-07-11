using ChatTask.BLL.Models;

namespace ChatTask.BLL.Services;

public interface IUserService
{
    Task<(Guid, Exception?)> Register(string username, string password);
    Task<Guid> Login(string username, string password);
}