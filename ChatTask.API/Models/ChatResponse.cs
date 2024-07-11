using ChatTask.BLL.Models;

namespace ChatTask.API.Models;

public class ChatResponse
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = null!;
    
    public List<Message> Messages { get; set; } = new();
    
    public DateTime CreatedAt { get; set; }
}