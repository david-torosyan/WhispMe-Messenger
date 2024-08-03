using WhispMe.BLL.AuthInterfaces;
using WhispMe.DAL.Entities;
using WhispMe.DAL.Interfaces;
using WhispMe.DTO;

namespace WhispMe.BLL.AuthServices;

public class AuthService : IAuthService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUnitOfWork _unitOfWork;

    public AuthService(IJwtTokenGenerator jwtTokenGenerator, IUnitOfWork unitOfWork)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _unitOfWork = unitOfWork;
    }

    public async Task<string> RegisterAsync(RegisterRequestDto registerRequestDto)
    {
        try
        {
            var salt = BCrypt.Net.BCrypt.GenerateSalt();
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerRequestDto.Password + salt);

            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                FullName = registerRequestDto.FullName,
                Email = registerRequestDto.Email,
                PasswordHash = passwordHash,
                Salt = salt,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.UserRepository.CreateAsync(user);
            return user.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred during registration.", ex);
        }
    }

    public async Task<string> LoginAsync(LoginRequestDto loginRequestDto)
    {
        try
        {
            var user = await _unitOfWork.UserRepository.GetByEmailAsync(loginRequestDto.Email)
                ?? throw new UnauthorizedAccessException("Invalid email or password");

            if (!BCrypt.Net.BCrypt.Verify(loginRequestDto.Password + user.Salt, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            return _jwtTokenGenerator.GenerateToken(user, user.Roles);
        }
        catch (Exception ex) when (ex is NotFoundException || ex is UnauthorizedAccessException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred during login.", ex);
        }
    }
}
