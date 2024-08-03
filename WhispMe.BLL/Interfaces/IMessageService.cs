using WhispMe.DTO.DTOs;

namespace WhispMe.BLL.Interfaces;

public interface IMessageService : IBaseService<MessageDto>
{
    Task<IEnumerable<MessageDto>> GetMessagesByRoomNameAsync(string roomName);
}
