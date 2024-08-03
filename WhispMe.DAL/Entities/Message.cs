namespace WhispMe.DAL.Entities;
public class Message
{
    public string? Id { get; set; }

    public string Content { get; set; }

    public DateTime Timestamp { get; set; }

    public string SenderEmail { get; set; }

    public string SenderFullName { get; set; }

    public string Room { get; set; }
}
