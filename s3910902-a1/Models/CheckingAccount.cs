namespace s3910902_a1.Models;

public class CheckingAccount : AbstractAccount
{
    public CheckingAccount()
    {
        AvailableBalance = Balance - 300M;
    }

    public override bool Debit(decimal amount)
    {
        if (amount <= 0 || AvailableBalance <= 300M) return false;

        Balance -= _accountPersistence.UpdateBalance(AccountNo, amount);
        return true;
    }
}