namespace ChatTask.Data.Entities;

public class UserEntity
{
    public Guid Id { get; set; }
    
    public string Username { get; set; } = null!;
    
    public string PasswordHash { get; set; } = null!;
}