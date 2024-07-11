using ChatTask.API.Hubs;
using ChatTask.BLL.Interfaces;
using ChatTask.BLL.Services;
using ChatTask.Data;
using ChatTask.Data.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder.AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .SetIsOriginAllowed((host) => true));
});



builder.Services.AddSignalR();
builder.Services.AddDbContext<ChatDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IUserService, UserService>();

builder.Services.AddTransient<IPasswordHasher, PasswordHasher>();

builder.Services.AddTransient<IChatRepository, ChatRepository>();
builder.Services.AddTransient<IChatService, ChatService>();

builder.Services.AddTransient<IMessageRepository, MessageRepository>();
builder.Services.AddTransient<IMessageService, MessageService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.MapControllers();

app.MapHub<ChatHub>("/chat-hub");

app.Run();