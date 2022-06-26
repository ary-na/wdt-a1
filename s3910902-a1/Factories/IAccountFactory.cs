using s3910902_a1.Models;

namespace s3910902_a1.Factories;

public interface IAccountFactory
{
    public IAccount CreateAccount(string accountType);
}