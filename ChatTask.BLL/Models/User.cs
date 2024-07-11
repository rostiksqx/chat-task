namespace ChatTask.BLL.Models;

public class User
{
    public Guid Id { get; set; }
    
    public string Username { get; set; } = null!;
    
    public string PasswordHash { get; set; } = null!;
    
    public List<Chat> Chats { get; set; } = new();
}