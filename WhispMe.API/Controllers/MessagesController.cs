using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace WhispMe.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class MessagesController : ControllerBase
{
    private readonly IMapper _mapper;
}
