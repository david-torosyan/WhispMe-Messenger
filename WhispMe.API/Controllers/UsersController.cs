using Microsoft.AspNetCore.Mvc;
using WhispMe.BLL.AuthInterfaces;
using WhispMe.DTO.DTOs;

namespace WhispMe.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<UsersController> _logger;
    public UsersController(IAuthService authService, ILogger<UsersController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<LoginResponseDto>> LoginAsync([FromBody] LoginRequestDto request)
    {
        if (request == null)
        {
            _logger.LogInformation("LoginAsync: Invalid login request.");
            return BadRequest("Invalid request data.");
        }

        var result = await _authService.LoginAsync(request);

        if (result == null)
        {
            _logger.LogWarning("LoginAsync: Login attempt failed for user {Email}.", request.Email);
            return BadRequest("Login failed.");
        }

        _logger.LogInformation("LoginAsync: Login successful for user {Email}.", request.Email);
        return Ok(result);
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequestDto request)
    {
        if (request == null)
        {
            _logger.LogInformation("RegisterAsync: Invalid registration request.");
            return BadRequest("Invalid request data.");
        }

        var result = await _authService.RegisterAsync(request);

        if (result == null)
        {
            _logger.LogWarning("RegisterAsync: Failed to register user {Name}.", request.FullName);
            return BadRequest("Registration failed.");
        }

        _logger.LogInformation("RegisterAsync: User {Name} registered successfully.", request.FullName);
        return Ok(result);
    }
}
