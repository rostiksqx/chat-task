using ChatTask.BLL.Interfaces;
using ChatTask.BLL.Models;
using ChatTask.Data.Repositories;

namespace ChatTask.BLL.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<(Guid, Exception?)> Register(string username, string password)
    {
        var existingUser = await _userRepository.GetUserByUserName(username);

        if (existingUser != null)
        {
            throw new Exception("User already exists");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = username,
            PasswordHash = _passwordHasher.HashPassword(password)
        };
        
        return await _userRepository.Add(user);
    }

    public async Task<Guid> Login(string username, string password)
    {
        var existingUser = await _userRepository.GetUserByUserName(username);

        if (existingUser == null)
        {
            throw new Exception("User does not exist");
        }

        if (!_passwordHasher.VerifyPassword(password, existingUser.PasswordHash))
        {
            throw new Exception("Invalid credentials");
        }

        return existingUser.Id;
    }
}