using ChatTask.BLL.Models;
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
    
    public DbSet<MessageEntity> Messages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChatEntity>()
            .HasMany(c => c.Messages)
            .WithOne(m => m.Chat)
            .HasForeignKey(m => m.ChatId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}