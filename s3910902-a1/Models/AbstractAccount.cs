using System.Net.Security;
using System.Transactions;
using s3910902_a1.Factories;
using s3910902_a1.Persistence;

namespace s3910902_a1.Models;

// Code sourced and adapted from:
// Week 3 Lectorial - Cat.cs
// https://rmit.instructure.com/courses/102750/files/24463725?wrap=1

public abstract class AbstractAccount : IAccount
{
    protected readonly IAccountPersistence _accountPersistence;
    private const int FreeTransactionCount = 2;
    protected const decimal MinBalanceChecking = 300M;
    public int CustomerId { get; set; }
    public int AccountNo { get; set; }
    public AccountType AccountType { get; set; }
    public List<ITransaction>? Transactions { get; set; }

    public decimal Balance { get; set; }

    public decimal AvailableBalance { get; set; }

    protected AbstractAccount()
    {
        _accountPersistence = new AccountPersistence();
    }

    public abstract bool Credit(decimal amount);

    public abstract bool Debit(decimal amount);

    // Code sourced and adapted from:
    // https://resharper-support.jetbrains.com/hc/en-us/community/posts/4403185593234
    
    public void AddTransaction(ITransaction transaction)
    {
        // Add transaction 
        Transactions?.Add(_accountPersistence.InsertTransaction(transaction));

        // Add service transaction
        if (FreeTransactionCount <= _accountPersistence.CountTransactions(transaction.AccountNumber) &&
            transaction.TransactionType is TransactionType.Transfer or TransactionType.Withdraw)
        {
            var service = new Service(transaction.TransactionType, transaction.AccountNumber);
            Transactions?.Add(_accountPersistence.InsertTransaction(service));
            Balance = _accountPersistence.UpdateBalance(transaction.AccountNumber, Balance - service.Amount);
            Balance -= service.Amount;
        }

        if (transaction.TransactionType != TransactionType.Transfer)
            return;

        // Add incoming transfer transaction
        var deposit = new Deposit
        {
            TransactionType = TransactionType.Deposit,
            AccountNumber = transaction.DestinationAccountNumber ?? throw new NullReferenceException(),
            Amount = transaction.Amount,
            TransactionTimeUtc = DateTime.UtcNow
        };
        Transactions?.Add(_accountPersistence.InsertTransaction(deposit));
    }
}