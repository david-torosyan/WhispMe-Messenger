using WhispMe.DAL.Entities;

namespace WhispMe.DAL.Data;

internal static class CollectionConstants
{
    public static readonly Dictionary<Type, string> Collections = new()
    {
        { typeof(User), "Users" },
        { typeof(Message), "Messages" },
        { typeof(Role), "Roles" },
        { typeof(Room), "Rooms" }
    };
}
