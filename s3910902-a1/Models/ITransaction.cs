namespace s3910902_a1.Models;

public interface ITransaction
{
    public int TransactionId { get; set; }
    public TransactionType TransactionType { get; set; }
    public int AccountNumber { get; set; }
    public int? DestinationAccountNumber { get; set; }
    public decimal Amount { get; set; }
    public string? Comment { get; set; }
    public DateTime TransactionTimeUtc { get; set; }
    public void Modifies();
}