using System.ComponentModel.DataAnnotations;

namespace WhispMe.WEB.Models;

public class LoginViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [DataType(DataType.Password)]
    public string Password { get; set; }

    public bool RememberMe { get; set; } = false;
}
