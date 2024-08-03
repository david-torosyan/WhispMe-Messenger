using MongoDB.Driver;
using WhispMe.DAL.Data;
using WhispMe.DAL.Entities;
using WhispMe.DAL.Interfaces;

namespace WhispMe.DAL.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    private readonly IMongoCollection<User> _collection;

    public UserRepository(WhispMeDbContext dbcontext)
        : base(dbcontext)
    {
        _collection = dbcontext.GetCollection<User>();
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        return await _collection.Find(Builders<User>.Filter.Eq(x => x.Email, email)).FirstOrDefaultAsync();
    }
}
