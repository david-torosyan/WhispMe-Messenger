using WhispMe.DAL.Entities;

namespace WhispMe.DAL.Interfaces;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User> GetByEmailAsync(string email);
}
