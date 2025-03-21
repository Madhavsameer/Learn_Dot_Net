using System.ComponentModel.DataAnnotations;

public class RegisterDto
{
    [Required]
    public string Name { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public string Role { get; set; } // "Admin" or "User"
}
