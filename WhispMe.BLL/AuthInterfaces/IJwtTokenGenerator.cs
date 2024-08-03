using WhispMe.DAL.Entities;

namespace WhispMe.BLL.AuthInterfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User applicationUser, IEnumerable<string> roles);
}
