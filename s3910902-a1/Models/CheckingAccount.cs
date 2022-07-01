namespace s3910902_a1.Models;

public class CheckingAccount : AbstractAccount
{
    public override bool UpdateBalance(decimal amount)
    {
        if (amount <= 0) return false;

        // Update balance and available balance
        Balance += amount;
        AvailableBalance = Balance < MinBalanceChecking ? 0M : Balance - MinBalanceChecking;
        return true;
    }

    public override bool Credit(decimal amount)
    {
        if (amount <= 0) return false;

        // Update balance and available balance
        Balance = AccountPersistence.UpdateBalance(AccountNo, amount + Balance);
        AvailableBalance = Balance < MinBalanceChecking ? 0M : Balance - MinBalanceChecking;
        return true;
    }

    public override bool Debit(decimal amount)
    {
        if (amount <= 0 || Balance - amount < 300M) return false;

        Balance = AccountPersistence.UpdateBalance(AccountNo, Balance - amount);
        AvailableBalance = Balance < MinBalanceChecking ? 0M : Balance - MinBalanceChecking;
        return true;
    }
}