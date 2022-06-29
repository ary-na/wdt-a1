namespace s3910902_a1.Utilities;

public static class Validation
{
    public static bool SelectedAccount(this string selectedAccount)
    {
        if (!int.TryParse(selectedAccount, out var input) || !input.IsInRange(1, 2))
        {
            "Invalid input.".ConsoleColorRed();
            return false;
        }
        return true;
    }
}