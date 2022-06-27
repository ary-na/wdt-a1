namespace s3910902_a1.DTOs;

public class TransactionDto
{
    public readonly char TransactionType = 'D';
    public int AccountNumber { get; set; }
    public decimal Amount { get; set; }
    public string? Comment { get; set; }
    public DateTime TransactionTimeUtc { get; set; }
}