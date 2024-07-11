namespace ChatTask.Data.Entities;

public class MessageEntity
{
    public Guid Id { get; set; }
    
    public Guid ChatId { get; set; }
    
    public ChatEntity Chat { get; set; } = null!;
    
    public Guid SenderId { get; set; }
    
    public string Text { get; set; }
}