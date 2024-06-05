using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignalChat.Core.Contracts;
using SignalChat.Models;

namespace SignalChat.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MessageController : ControllerBase
{
    private readonly IMessageRepository _messageRepository;

    public MessageController(IMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }

    [HttpGet]
    public async Task<IEnumerable<MessageViewModel>> GetAsync()
    {
        var mostRecent = await _messageRepository.FindMostRecentAsync();
        var messages = mostRecent.Select(t => new MessageViewModel
        {
            Username = t.Username,
            Body = t.Body,
            DeliveredAt = t.DeliveredAt
        });

        return messages;
    }
}