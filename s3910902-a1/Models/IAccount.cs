namespace s3910902_a1.Models;

public interface IAccount
{
    public int CustomerId { get; set; } 
    public int AccountNo { get; set; }
    public AccountType AccountType { get; set; }
    public decimal Balance { get; }
    public decimal AvailableBalance { get; set; }
    public List<ITransaction>? Transactions { get; set; }
    public bool Credit(decimal amount);
    public bool Debit(decimal amount);

    public void AddTransaction(ITransaction transaction);
}