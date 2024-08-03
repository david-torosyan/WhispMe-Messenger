using WhispMe.DAL.Entities;

namespace WhispMe.DAL.Interfaces;

public interface IRoomRepository : IBaseRepository<Room>
{
    Task<Room> GetByNameAsync(string roomName);
}
