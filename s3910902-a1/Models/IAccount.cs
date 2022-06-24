namespace s3910902_a1.Models;

public interface IAccount
{
    public int AccountNo { get; set; }
    public char AccountType { get; set; }
    public decimal Balance { get; set; }
    public List<ITransaction>? Transactions { get; set; }
    public void Credit();
    public decimal Debit();
}