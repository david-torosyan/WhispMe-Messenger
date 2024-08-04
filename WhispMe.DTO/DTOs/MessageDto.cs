using System.ComponentModel.DataAnnotations;

namespace WhispMe.DTO.DTOs;

public class MessageDto
{
    public string Id { get; set; }

    [Required]
    public string Content { get; set; }

    public DateTime Timestamp { get; set; }

    public string SenderEmail { get; set; }

    public string SenderFullName { get; set; }

    public string Room { get; set; }

    public string Avatar { get; set; }
}
