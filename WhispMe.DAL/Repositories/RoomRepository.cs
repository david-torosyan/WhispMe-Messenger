using MongoDB.Driver;
using WhispMe.DAL.Data;
using WhispMe.DAL.Entities;
using WhispMe.DAL.Interfaces;

namespace WhispMe.DAL.Repositories;

public class RoomRepository : BaseRepository<Room>, IRoomRepository
{
    private readonly IMongoCollection<Room> _collection;

    public RoomRepository(WhispMeDbContext dbcontext)
        : base(dbcontext)
    {
        _collection = dbcontext.GetCollection<Room>();
    }

    public async Task<Room> GetByNameAsync(string roomName)
    {
        return await _collection.Find(Builders<Room>.Filter.Eq(x => x.Name, roomName)).FirstOrDefaultAsync();
    }
}
