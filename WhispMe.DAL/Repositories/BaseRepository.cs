using MongoDB.Driver;
using WhispMe.DAL.Data;
using WhispMe.DAL.Interfaces;

namespace WhispMe.DAL.Repositories;

public class BaseRepository<T> : IBaseRepository<T>
    where T : class, new()
{
    private readonly IMongoCollection<T> _collection;
    private readonly IMongoDatabase _database;

    public BaseRepository(WhispMeDbContext dbcontext)
    {
        _collection = dbcontext.GetCollection<T>();
        _database = dbcontext.GetDatabase();
    }

    public async Task<T> GetByIdAsync(string id)
    {
        return await _collection.Find(Builders<T>.Filter.Eq("_id", id)).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _collection.Find(Builders<T>.Filter.Empty).ToListAsync();
    }

    public async Task CreateAsync(T entity)
    {
        await _collection.InsertOneAsync(entity);
    }

    public async Task UpdateAsync(string id, T entity)
    {
        await _collection.ReplaceOneAsync(Builders<T>.Filter.Eq("_id", id), entity);
    }

    public async Task DeleteAsync(string id)
    {
        await _collection.DeleteOneAsync(Builders<T>.Filter.Eq("_id", id));
    }
}
