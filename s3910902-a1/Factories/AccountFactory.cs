using s3910902_a1.Models;

namespace s3910902_a1.Factories;

public class AccountFactory : IAccountFactory
{
    public AccountFactory(string accountType)
    {
        accountType switch
        {
            "S" => new SavingAccount(),
            "C" => new CheckingAccount(),
            _ => throw new NullReferenceException()
        };
    }
    
    public IAccount CreateAccount(string accountType)
    {
        return accountType switch
        {
            "S" => new SavingAccount(),
            "C" => new CheckingAccount(),
            _ => throw new NullReferenceException()
        };
    }
}