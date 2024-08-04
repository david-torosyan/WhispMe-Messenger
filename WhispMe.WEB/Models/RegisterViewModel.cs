﻿using System.ComponentModel.DataAnnotations;

namespace WhispMe.WEB.Models;

public class RegisterViewModel
{
    [Required]
    public string FullName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [DataType(DataType.Password)]
    public string Password { get; set; }
}
