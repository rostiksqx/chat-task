using ChatTask.BLL.Models;
using ChatTask.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatTask.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ChatDbContext _dbContext;

    public UserRepository(ChatDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<User?> GetUserByUserName(string username)
    {
        var userEntity = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
        
        if (userEntity == null)
        {
            return null;
        }

        var user = new User
        {
            Id = userEntity.Id,
            Username = userEntity.Username,
            PasswordHash = userEntity.PasswordHash
        };
        
        return user;
    }

    public async Task<(Guid, Exception?)> Add(User user)
    {
        var userEntity = new UserEntity
        {
            Id = user.Id,
            Username = user.Username,
            PasswordHash = user.PasswordHash
        };
        
        _dbContext.Users.Add(userEntity);
        var ex = await _dbContext.SafeSave();
        
        if (ex != null)
        {
            return (Guid.Empty, ex);
        }
        
        return (userEntity.Id, null);
    }

    public async Task<User?> GetUserById(Guid id)
    {
        var userEntity = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        
        if (userEntity == null)
        {
            return null;
        }
        
        var user = new User
        {
            Id = userEntity.Id,
            Username = userEntity.Username,
            PasswordHash = userEntity.PasswordHash
        };
        
        return user;
    }
}