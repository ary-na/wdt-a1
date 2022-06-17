namespace s3910902_a1.Dto;

public class TransactionDto
{
    public int TransactionId { get; set; }
    public string? TransactionType { get; set; }
    public int AccountNumber { get; set; }
    public int? DestinationAccountNumber { get; set; }
    public decimal Amount { get; set; }
    public string? Comment { get; set; }
    public DateTime TransactionTimeUtc { get; set; }
}