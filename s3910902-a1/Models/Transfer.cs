namespace s3910902_a1.Models;

public class Transfer : AbstractTransaction
{
    public Transfer()
    {
        TransactionType = TransactionType.Transfer;
        TransactionTimeUtc = DateTime.UtcNow;
    }

    public Transfer(int accountNumber, int destinationAccountNumber, decimal amount, string? comment) : base(accountNumber, amount, comment)
    {
        TransactionType = TransactionType.Transfer;
        DestinationAccountNumber = destinationAccountNumber;
    }
}