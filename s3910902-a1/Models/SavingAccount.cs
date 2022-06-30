namespace s3910902_a1.Models;

public class SavingAccount : AbstractAccount
{
    public override bool Credit(decimal amount)
    {
        if (amount <= 0) return false;

        Balance = AccountPersistence.UpdateBalance(AccountNo, amount + Balance);
        AvailableBalance = Balance;
        return true;
    }

    public override bool Debit(decimal amount)
    {
        if (amount <= 0 || Balance - amount <= 0)
            return false;

        Balance = AccountPersistence.UpdateBalance(AccountNo, Balance - amount);
        AvailableBalance = Balance;
        return true;
    }
}