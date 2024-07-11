namespace ChatTask.Data.Entities;

public class ChatEntity
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = null!;
    
    public Guid CreatorId { get; set; }
    
    public UserEntity Creator { get; set; } = null!;
    
    public List<MessageEntity> Messages { get; set; } = new();
    
    public DateTime CreatedAt { get; set; }
}