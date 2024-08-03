namespace WhispMe.DAL.Entities;

public class User
{
    public string Id { get; set; }

    public string FullName { get; set; }

    public string Avatar { get; set; }

    public string Email { get; set; }

    public string PasswordHash { get; set; }

    public string Salt { get; set; }

    public List<string> Roles { get; set; }

    public DateTime CreatedAt { get; set; }
}
