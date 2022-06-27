namespace s3910902_a1.DTOs;

public class CustomerDto
{
    public int CustomerId { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? PostCode { get; set; }
    public AccountDto[]? Accounts { get; set; }
    public LoginDto? Login { get; set; }
}