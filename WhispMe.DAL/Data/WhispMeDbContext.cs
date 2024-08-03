using MongoDB.Driver;
using WhispMe.DAL.Entities;

namespace WhispMe.DAL.Data;

public class WhispMeDbContext(IMongoClient mongoClient, string databaseName)
{
    private readonly IMongoDatabase _database = mongoClient.GetDatabase(databaseName);

    public IMongoCollection<User> Users => _database.GetCollection<User>("Users");

    public IMongoCollection<Message> Messages => _database.GetCollection<Message>("Messages");

    public IMongoCollection<Role> Whisps => _database.GetCollection<Role>("Roles");

    public IMongoCollection<Room> WhispMessages => _database.GetCollection<Room>("Rooms");

    public IMongoCollection<T> GetCollection<T>()
    {
        return _database.GetCollection<T>(CollectionConstants.Collections[typeof(T)]);
    }

    public IMongoDatabase GetDatabase()
    {
        return _database;
    }
}
