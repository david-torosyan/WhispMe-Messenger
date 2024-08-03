using WhispMe.DTO;

namespace WhispMe.BLL.AuthInterfaces;

public interface IAuthService
{
    Task<string> LoginAsync(LoginRequestDto loginRequestDto);

    Task<string> RegisterAsync(RegisterRequestDto registerRequestDto);
}
