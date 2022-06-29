namespace Utilities.DTOs;

public class LoginDto
{
    public string? LoginId { get; set; }
    public int CustomerId { get; set; }
    public string? PasswordHash { get; set; }
}