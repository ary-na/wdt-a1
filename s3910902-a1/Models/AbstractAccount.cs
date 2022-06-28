using s3910902_a1.Persistence;

namespace s3910902_a1.Models;

public abstract class AbstractAccount : IAccount
{
    protected readonly IAccountPersistence _accountPersistence;
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

    public bool Credit(decimal amount)
    {
        if (amount <= 0) return false;

        Balance = _accountPersistence.UpdateBalance(AccountNo, amount + Balance);
        return true;
    }

    public abstract bool Debit(decimal amount);

    public void AddTransaction(ITransaction transaction)
    {
        Transactions?.Add(_accountPersistence.InsertTransaction(transaction));
    }
}