using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WhispMe.API.Hubs;
using WhispMe.BLL.Interfaces;
using WhispMe.DAL.Entities;
using WhispMe.DTO.DTOs;

namespace WhispMe.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoomsController : ControllerBase
{
    private readonly IRoomService _roomService;
    private readonly IHubContext<ChatHub> _hubContext;

    public RoomsController(IRoomService roomService, IHubContext<ChatHub> hubContext)
    {
        _roomService = roomService;
        _hubContext = hubContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoomDto>>> Get()
    {
        var rooms = await _roomService.GetWithPaginationAsync(1, 100);

        return Ok(rooms);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Room>> Get(string id)
    {
        var room = await _roomService.GetByIdAsync(id);
        return Ok(room);
    }

    [HttpPost]
    public async Task<ActionResult<Room>> Create(RoomDto viewModel)
    {
        var createdRoom = await _roomService.CreateAsync(viewModel);

        await _hubContext.Clients.All.SendAsync("addChatRoom", createdRoom);

        return CreatedAtAction(nameof(Get), new { id = createdRoom.Id }, createdRoom);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Edit(string id, RoomDto viewModel)
    {
        if (id != viewModel.Id)
            return BadRequest();

        var updatedRoom = await _roomService.UpdateAsync(id, viewModel);

        await _hubContext.Clients.All.SendAsync("updateChatRoom", updatedRoom);

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var room = await _roomService.DeleteByIdAsync(id);

        if (room == null)
            return NotFound();

        await _hubContext.Clients.All.SendAsync("removeChatRoom", room.Id);
        await _hubContext.Clients.Group(room.Name).SendAsync("onRoomDeleted");

        return Ok();
    }
}
