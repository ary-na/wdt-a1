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
    protected AbstractTransaction(TransactionType transactionType, int accountNumber, decimal amount, string? comment, DateTime transactionTimeUtc)
    {
        TransactionType = transactionType;
        AccountNumber = accountNumber;
        Amount = amount;
        Comment = comment;
        TransactionTimeUtc = transactionTimeUtc;
    }
    
    public void Modifies()
    {
        throw new NotImplementedException();
    }
}