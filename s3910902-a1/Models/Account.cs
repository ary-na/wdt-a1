namespace s3910902_a1.Model;

public abstract class AbstractAccount : IAccount
{
    public int AccountNo { get; set; }
    public AccountType AccountType { get; set; }
    public int OwnerCustomerId { get; set; }
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