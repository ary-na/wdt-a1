namespace s3910902_a1.Models;

public class Withdraw : AbstractTransaction
{
    public Withdraw()
    { }

    public Withdraw(int accountNumber, decimal amount, string? comment) : base(accountNumber, amount, comment)
    {
        TransactionType = TransactionType.Withdraw;
    }
}