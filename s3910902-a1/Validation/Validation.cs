using s3910902_a1.Persistence;
using Utilities.ExtensionMethods;

namespace s3910902_a1.Validation;

public static class UtilityMethods
{
    private static readonly IAccountPersistence AccountPersistence = new AccountPersistence();

    public static bool IsValidAccount(this string validAccount)
    {
        if (int.TryParse(validAccount, out var selectedAccount) && selectedAccount.IsInRange(1, 2))
            return true;

        "Invalid input.".ConsoleColorRed();
        return false;
    }

    public static bool IsValidDestinationAccount(this string validDestinationAccount, int accountNumber)
    {
        if (int.TryParse(validDestinationAccount, out var destinationAccount) &&
            AccountPersistence.ValidAccountNumber(destinationAccount) > 0 && accountNumber != destinationAccount)
            return true;

        "Invalid account number.".ConsoleColorRed();
        return false;
    }

    public static bool IsValidAmount(this string validAmount)
    {
        if (decimal.TryParse(validAmount, out var amount) && amount > 0)
            return true;

        "Incorrect amount.".ConsoleColorRed();
        return false;
    }

    public static bool IsValidComment(this string validComment)
    {
        if (validComment.Length < 30)
            return true;

        "Comment exceeded maximum length.".ConsoleColorRed();
        return false;
    }

    public static bool IsNullComment(this string comment)
    {
        return string.IsNullOrWhiteSpace(comment) || comment.Equals("n");
    }
}