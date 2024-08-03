using MongoDB.Driver;
using WhispMe.DAL.Data;
using WhispMe.DAL.Interfaces;

namespace WhispMe.DAL.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly WhispMeDbContext _context;

    public UnitOfWork(WhispMeDbContext context)
    {
        _context = context;
    }

    public IUserRepository UserRepository => new UserRepository(_context);

    public IMessageRepository MessageRepository => new MessageRepository(_context);

    public IRoleRepository RoleRepository => new RoleRepository(_context);

    public IRoomRepository RoomRepository => new RoomRepository(_context);
}
