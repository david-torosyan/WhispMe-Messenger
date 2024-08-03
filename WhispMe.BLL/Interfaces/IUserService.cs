using WhispMe.DTO.DTOs;

namespace WhispMe.BLL.Interfaces;

public interface IUserService : IBaseService<UserDto>
{
    Task<UserDto> GetUserByIdentityName(string identityName);
}
