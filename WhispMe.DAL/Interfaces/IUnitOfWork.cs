namespace WhispMe.DAL.Interfaces;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }

    IMessageRepository MessageRepository { get; }

    IRoleRepository RoleRepository { get; }

    IRoomRepository RoomRepository { get; }
}
