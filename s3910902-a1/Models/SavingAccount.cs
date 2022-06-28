namespace s3910902_a1.Models;

public class SavingAccount : AbstractAccount
{
    public SavingAccount()
    {
        AvailableBalance = Balance;
    }

    public override bool Debit(decimal amount)
    {
        if (amount <= 0 || Balance <= 0)
            return false;

        Balance -= _accountPersistence.UpdateBalance(AccountNo, amount);;
        return true;
    }
}