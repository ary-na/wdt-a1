namespace s3910902_a1.Models;

public class Deposit : AbstractTransaction
{
    public Deposit()
    { }

    public Deposit(TransactionType transactionType, int accountNumber, decimal amount, string? comment,
        DateTime transactionTimeUtc)
        : base(transactionType, accountNumber, amount, comment, transactionTimeUtc)
    { }
}