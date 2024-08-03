using MongoDB.Driver;
using WhispMe.DAL.Data;
using WhispMe.DAL.Entities;
using WhispMe.DAL.Interfaces;

namespace WhispMe.DAL.Repositories;

public class RoleRepository : BaseRepository<Role>, IRoleRepository
{
    public RoleRepository(WhispMeDbContext dbcontext)
        : base(dbcontext)
    {
    }
}
