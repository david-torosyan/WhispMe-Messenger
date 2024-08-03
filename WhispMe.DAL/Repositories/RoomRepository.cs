using WhispMe.DAL.Data;
using WhispMe.DAL.Entities;
using WhispMe.DAL.Interfaces;

namespace WhispMe.DAL.Repositories;

public class RoomRepository : BaseRepository<Room>, IRoomRepository
{
    public RoomRepository(WhispMeDbContext dbcontext)
        : base(dbcontext)
    {
    }
}
