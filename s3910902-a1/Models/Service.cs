namespace s3910902_a1.Models;

// Code sourced and adapted from:
// https://blog.udemy.com/decimal-in-c-sharp/

public class Service : AbstractTransaction
{
    private const decimal WithdrawFee = 0.05M;
    private const decimal TransferFee = 0.10M;

    public Service()
    { }

    public Service(int accountNumber, decimal amount, string? comment) : base(accountNumber, amount, comment)
    { }

    public Service(TransactionType withdrawOrTransfer, int accountNumber)
    {
        TransactionType = TransactionType.Service;
        AccountNumber = accountNumber;
        Amount = withdrawOrTransfer == TransactionType.Withdraw ? WithdrawFee : TransferFee;
        TransactionTimeUtc = DateTime.UtcNow;
    }
}