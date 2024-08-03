using MongoDB.Driver;
using WhispMe.DAL.Data;
using WhispMe.DAL.Entities;
using WhispMe.DAL.Interfaces;

namespace WhispMe.DAL.Repositories;

public class MessageRepository : BaseRepository<Message>, IMessageRepository
{
    private readonly IMongoCollection<Message> _collection;

    public MessageRepository(WhispMeDbContext dbcontext)
        : base(dbcontext)
    {
        _collection = dbcontext.GetCollection<Message>();
    }

    public async Task<IEnumerable<Message>> GetByRoomAsync(string room)
    {
        return await _collection.Find(x => x.Room == room).ToListAsync();
    }
}
