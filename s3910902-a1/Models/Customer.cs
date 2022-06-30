namespace s3910902_a1.Models;

public class Customer
{
    public int CustomerId { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? PostCode { get; set; }
    public IAccount[]? Accounts { get; set; }
    public Login? Login { get; set; }
}