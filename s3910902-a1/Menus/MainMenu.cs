using s3910902_a1.Mangers;
using s3910902_a1.Models;
using s3910902_a1.Utilities;

namespace s3910902_a1.Menus;

public static class MainMenu
{
    // Code sourced and adapted from:
    // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/statements/selection-statements#code-try-0
    // https://www.geeksforgeeks.org/switch-statement-in-c-sharp/

    private static readonly ModelManger ModelManger = new();
    private static CustomerManager? _customerManager;

    // Run menu
    public static void Run(LoginManager loginManager)
    {
        _customerManager = new CustomerManager(ModelManger.ConnectionString, loginManager);
        var exit = false;
        do
        {
            PrintMenu();
            switch (ReadInput())
            {
                case '1':
                    Deposit();
                    break;
                case '2':
                    Withdraw();
                    break;
                case '3':
                    Transfer();
                    break;
                case '4':
                    MyStatement();
                    break;
                case '5':
                    Logout();
                    exit = true;
                    break;
                case '6':
                    Exit();
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Please select a valid menu option.");
                    Console.WriteLine();
                    break;
            }
        } while (!exit);
    }

    // Code sourced and adapted from:
    // Week 3 Lectorial - FormatStrings.cs
    // https://rmit.instructure.com/courses/102750/files/24463725?wrap=1

    // Print menu
    private static void PrintMenu()
    {
        var separator = new string('-', 29);
        Console.WriteLine(separator);
        Console.WriteLine("Most Common Bank of Australia");
        Console.WriteLine(separator);
        Console.WriteLine();
        Console.WriteLine($"--- {_customerManager?.Customer?.Name} ---");
        Console.WriteLine();
        Console.WriteLine("[1] Deposit");
        Console.WriteLine("[2] Withdraw");
        Console.WriteLine("[3] Transfer");
        Console.WriteLine("[4] My Statement");
        Console.WriteLine("[5] Logout");
        Console.WriteLine("[6] Exit");
        Console.WriteLine();
        Console.Write("Enter an option: ");
    }

    // Code sourced and adapted from:
    // https://www.geeksforgeeks.org/different-methods-to-read-a-character-in-c-sharp/

    private static char ReadInput()
    {
        // Read user input as character
        var input = Console.ReadKey().KeyChar;
        Console.WriteLine();
        return input;
    }

    // Code sourced and adapted from:
    // Week 1 Lectorial - Person.cs
    // https://rmit.instructure.com/courses/102750/files/25011410?wrap=1

    // Menu methods
    private static void Deposit()
    {
        Console.WriteLine();
        var accountCounter = 1;
        Console.WriteLine("--- Deposit ---");
        Console.WriteLine();

        foreach (var account in _customerManager.Customer.Accounts)
        {
            Console.WriteLine(
                $"{accountCounter}. {account.AccountType,-15}{account.AccountNo,-15}{account.Balance,-15:C}");
            accountCounter++;
        }

        Console.WriteLine();
        if (!int.TryParse("Select an account: ".ReadInput(), out var input) || !input.IsInRange(1, 2))
        {
            "Invalid input".ConsoleColorRed();
            return;
        }

        var selectedAccount =
            input is 1 ? _customerManager.Customer.Accounts[0] : _customerManager.Customer.Accounts[1];
        Console.WriteLine();
        Console.WriteLine(
            $"{selectedAccount.AccountType} " +
            $"{selectedAccount.AccountNo} " +
            $"Balance: {selectedAccount.Balance:C} " +
            $"Available Balance: {selectedAccount.Balance:C}");

        if (decimal.TryParse("Enter amount: ".ReadInput(), out var amount) && amount <= 0)
        {
            "Amount cannot be negative.".ConsoleColorRed();
            return;
        }

        var comment = "Enter comment (n to quit, max length 30): ".ReadInput();
        if (string.IsNullOrWhiteSpace(comment) || comment.Equals("n"))
            comment = null;

        var deposit = new Deposit(TransactionType.Deposit, selectedAccount.AccountNo, amount, comment, DateTime.UtcNow);
        selectedAccount.AddTransaction(deposit);
        selectedAccount.Credit(amount);
        
        Console.WriteLine($"Deposit of {amount:C} successful, account balance is now {selectedAccount.Balance:C}");
        Console.WriteLine();
    }

    private static void Withdraw() => Console.WriteLine("Withdraw");
    private static void Transfer() => Console.WriteLine("Transfer");
    private static void MyStatement() => Console.WriteLine("MyStatement");

    // Code sourced and adapted from:
    // https://docs.microsoft.com/en-us/dotnet/api/system.console.clear?view=net-6.0
    private static void Logout()
    {
        Console.Clear();
        _customerManager = null;
        LoginMenu.Run(new ModelManger());
    }

    private static void Exit() => Console.WriteLine("Program ending!");
}