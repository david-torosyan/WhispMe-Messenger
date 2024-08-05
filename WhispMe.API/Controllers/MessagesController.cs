using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WhispMe.API.Hubs;
using WhispMe.BLL.Interfaces;
using WhispMe.DAL.Entities;
using WhispMe.DTO.DTOs;

namespace WhispMe.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MessagesController : ControllerBase
{
    private readonly IMessageService _messageService;
    private readonly IHubContext<ChatHub> _hubContext;
    public MessagesController(IMessageService messageService, IHubContext<ChatHub> hubContext)
    {
        _messageService = messageService;
        _hubContext = hubContext;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Room>> Get(string id)
    {
        var message = await _messageService.GetByIdAsync(id);
        if (message == null)
            return NotFound();

        return Ok(message);
    }

    [HttpGet("Room/{roomName}")]
    public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessages(string roomName)
    {
        var messages = await _messageService.GetMessagesByRoomNameAsync(roomName);
        if (messages == null)
            return NotFound();

        return Ok(messages);
    }

    [HttpGet]
    [Route("PushMessage")]
    public async Task<IActionResult> PushMessage(string user, string message)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveMessage", user, message);

        return Ok("Done");
    }

    [HttpPost]
    public async Task<ActionResult<Message>> Create(MessageDto viewModel)
    {
        var msg = await _messageService.CreateAsync(viewModel);
        if (msg == null)
            return BadRequest();

        return Ok(msg);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var message = await _messageService.GetByIdAsync(id);
        if (message == null)
            return NotFound();

        await _messageService.DeleteByIdAsync(id);
        return NoContent();
    }
}
