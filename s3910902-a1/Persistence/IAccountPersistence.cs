using s3910902_a1.Models;

namespace s3910902_a1.Persistence;

public interface IAccountPersistence
{
    public ITransaction InsertTransaction(ITransaction transaction);
    public decimal UpdateBalance(int accountNumber, decimal balance);
}