using WhispMe.DAL.Entities;

namespace WhispMe.DAL.Interfaces;

public interface IMessageRepository : IBaseRepository<Message>
{
    Task<IEnumerable<Message>> GetByRoomAsync(string Room);
}
