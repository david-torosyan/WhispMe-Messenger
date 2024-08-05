using WhispMe.DTO.DTOs;

namespace WhispMe.BLL.AuthInterfaces;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequestDto);

    Task<string> RegisterAsync(RegisterRequestDto registerRequestDto);
}
