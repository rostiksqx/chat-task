﻿using ChatTask.BLL.Models;
using ChatTask.Data.Repositories;

namespace ChatTask.BLL.Services;

public class ChatService : IChatService
{
    private readonly IChatRepository _chatRepository;

    public ChatService(IChatRepository chatRepository)
    {
        _chatRepository = chatRepository;
    }
    
    public async Task<List<Chat>> GetAllChats()
    {
        return await _chatRepository.GetAllChats();
    }

    public async Task<Guid> CreateChat(string name, Guid creatorId)
    {
        var chat = new Chat
        {
            Id = Guid.NewGuid(),
            Name = name,
            CreatorId = creatorId,
            CreatedAt = DateTime.Now.ToUniversalTime()
        };
        
        return await _chatRepository.Add(chat);
    }

    public async Task<Guid> DeleteChat(Guid chatId, Guid creatorId)
    {
        var existingChat = await _chatRepository.GetChatById(chatId);
        
        if (existingChat == null)
        {
            throw new ArgumentException("Chat not found");
        }
        
        // if (existingChat.CreatorId != creatorId)
        // {
        //     throw new ArgumentException("You are not the creator of this chat");
        // }
        
        return await _chatRepository.Delete(chatId);
    }

    public async Task<Chat> GetChatByName(string chatName)
    {
        return await _chatRepository.GetChatByName(chatName) ?? throw new ArgumentException("Chat not found");
    }
    
    public async Task<List<Chat>> GetChatsByUserId(Guid userId)
    {
        return await _chatRepository.GetChatsByUserId(userId);
    }
}