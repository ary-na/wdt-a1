namespace s3910902_a1.Models;

// Code sourced and adapted from:
// https://blog.udemy.com/decimal-in-c-sharp/

public class Service : AbstractTransaction
{
    private const decimal WithdrawFee = 0.05M;
    private const decimal TransferFee = 0.10M;

    public Service()
    { }
    public Service(TransactionType transactionType, int accountNumber, decimal amount, string? comment,
        DateTime transactionTimeUtc) : base(transactionType, accountNumber, amount, comment, transactionTimeUtc)
    {
    }
}