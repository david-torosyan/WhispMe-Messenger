using MongoDB.Driver;
using WhispMe.DAL.Data;
using WhispMe.DAL.Entities;
using WhispMe.DAL.Interfaces;

namespace WhispMe.DAL.Repositories;

public class MessageRepository : BaseRepository<Message>, IMessageRepository
{
    public MessageRepository(WhispMeDbContext dbcontext)
        : base(dbcontext)
    {
    }
}
