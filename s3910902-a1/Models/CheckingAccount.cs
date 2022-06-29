namespace s3910902_a1.Models;

public class CheckingAccount : AbstractAccount
{
    public override bool Credit(decimal amount)
    {
        if (amount <= 0) return false;

        Balance = _accountPersistence.UpdateBalance(AccountNo, amount + Balance);
        AvailableBalance = Balance < MinBalanceChecking ? 0M : Balance - MinBalanceChecking;
        return true;
    }
    
    public override bool Debit(decimal amount)
    {
        if (amount <= 0 || Balance - amount < 300M) return false;

        Balance = _accountPersistence.UpdateBalance(AccountNo, Balance - amount);
        AvailableBalance = Balance < MinBalanceChecking ? 0M : Balance - MinBalanceChecking;
        return true;
    }
}