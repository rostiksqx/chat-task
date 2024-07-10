using ChatTask.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatTask.Data;

public class ChatDbContext : DbContext
{
    public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options) 
    {
    }
    
    public DbSet<UserEntity> Users { get; set; }
    
    public DbSet<ChatEntity> Chats { get; set; }
}