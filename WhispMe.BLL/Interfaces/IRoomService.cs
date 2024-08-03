using WhispMe.DTO.DTOs;

namespace WhispMe.BLL.Interfaces;

public interface IRoomService : IBaseService<RoomDto>
{
    Task<RoomDto> GetRoomByNameAycnc(string roomName);
}
