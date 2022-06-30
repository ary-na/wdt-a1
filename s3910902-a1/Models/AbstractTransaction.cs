namespace s3910902_a1.Models;

public abstract class AbstractTransaction : ITransaction
{
    public int TransactionId { get; set; }
    public TransactionType TransactionType { get; set; }
    public int AccountNumber { get; set; }
    public int? DestinationAccountNumber { get; set; }
    public decimal Amount { get; set; }
    public string? Comment { get; set; }
    public DateTime TransactionTimeUtc { get; set; }

    protected AbstractTransaction()
    { }

    protected AbstractTransaction(int accountNumber, decimal amount, string? comment)
    {
        AccountNumber = accountNumber;
        Amount = amount;
        Comment = comment;
        TransactionTimeUtc = DateTime.UtcNow;
    }

    public void Modifies(IAccount account, decimal amount)
    {
        account.Credit(amount);
    }
}