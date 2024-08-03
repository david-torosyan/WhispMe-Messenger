namespace WhispMe.DTO;

public class MessageDto
{
    public string Id { get; set; }

    public string Content { get; set; }

    public DateTime Timestamp { get; set; }

    public string SenderUserId { get; set; }

    public string ToRoomId { get; set; }
}
