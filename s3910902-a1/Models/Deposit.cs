namespace s3910902_a1.Models;

public class Deposit : AbstractTransaction
{
    public Deposit()
    {
    }

    public Deposit(int accountNumber, decimal amount, string? comment)
        : base(accountNumber, amount, comment)
    {
        TransactionType = TransactionType.Deposit;
    }
}