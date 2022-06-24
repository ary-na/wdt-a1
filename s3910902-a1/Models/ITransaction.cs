namespace s3910902_a1.Models;

public interface ITransaction
{
    public int TransactionId { get; set; }
    public char TransactionType { get; set; }
    public decimal Amount { get; set; }
    public string? Comment { get; set; }
    public DateTime TransactionTimeUtc { get; set; }
    public void Modifies();
}