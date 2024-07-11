﻿namespace ChatTask.BLL.Models;

public class Message
{
    public Guid Id { get; set; }
    
    public Guid ChatId { get; set; }
    
    public Guid SenderId { get; set; }
    
    public string Text { get; set; }
}