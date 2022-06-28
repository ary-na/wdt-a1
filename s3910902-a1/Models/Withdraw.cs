namespace s3910902_a1.Models;

public class Withdraw : AbstractTransaction
{
    public Withdraw()
    { }
    public Withdraw(TransactionType transactionType, int accountNumber, decimal amount, string? comment,
        DateTime transactionTimeUtc) : base(transactionType, accountNumber, amount, comment, transactionTimeUtc)
    {
    }
}