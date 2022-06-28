namespace s3910902_a1.Models;

public class Transfer : AbstractTransaction
{
    public Transfer()
    { }
    public Transfer(TransactionType transactionType, int accountNumber, int destinationAccountNumber , decimal amount, string? comment,
        DateTime transactionTimeUtc) : base(transactionType, accountNumber, amount, comment, transactionTimeUtc)
    {
        DestinationAccountNumber = destinationAccountNumber;
    }
}