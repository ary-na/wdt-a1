namespace s3910902_a1.Models;

public abstract class AbstractAccount : IAccount
{
    public int CustomerId { get; set; }
    public int AccountNo { get; set; }
    public AccountType AccountType { get; set; }
    
    public List<ITransaction>? Transactions { get; set; }
    public decimal Balance { get; set; }

    public void Credit()
    {
        throw new NotImplementedException();
    }

    public decimal Debit()
    {
        throw new NotImplementedException();
    }
}