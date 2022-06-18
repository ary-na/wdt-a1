namespace s3910902_a1.Dto;

public class AccountDto
{
    public int AccountNumber { get; set; }
    public char? AccountType { get; set; }
    public int CustomerId { get; set; }
    public decimal Balance { get; set; }
    public TransactionDto[]? Transactions { get; set; }
}