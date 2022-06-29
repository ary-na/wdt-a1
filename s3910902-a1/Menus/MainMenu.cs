using System.Transactions;
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

    // Code sourced and adapted from:
    // https://docs.microsoft.com/en-us/dotnet/api/system.string.isnullorwhitespace?view=net-6.0
    // https://docs.microsoft.com/en-us/dotnet/api/system.datetime.utcnow?view=net-6.0
    // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/conditional-operator
    // https://docs.microsoft.com/en-us/dotnet/api/system.int32.tryparse?view=net-6.0

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
            $"Available Balance: {selectedAccount.AvailableBalance:C}");

        if (decimal.TryParse("Enter amount: ".ReadInput(), out var amount) && amount <= 0)
        {
            "Amount cannot be negative.".ConsoleColorRed();
            return;
        }

        var comment = "Enter comment (n to quit, max length 30): ".ReadInput();
        if (string.IsNullOrWhiteSpace(comment) || comment.Equals("n"))
            comment = null;

        var deposit = new Deposit(selectedAccount.AccountNo, amount, comment);
        selectedAccount.AddTransaction(deposit);
        selectedAccount.Credit(amount);

        Console.WriteLine($"Deposit of {amount:C} successful, account balance is now {selectedAccount.Balance:C}");
        Console.WriteLine();
    }

    private static void Withdraw()
    {
        Console.WriteLine();
        var accountCounter = 1;
        Console.WriteLine("--- Withdraw ---");
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
            $"Available Balance: {selectedAccount.AvailableBalance:C}");

        if (decimal.TryParse("Enter amount: ".ReadInput(), out var amount) && amount <= 0)
        {
            "Amount cannot be negative.".ConsoleColorRed();
            return;
        }

        var comment = "Enter comment (n to quit, max length 30): ".ReadInput();
        if (string.IsNullOrWhiteSpace(comment) || comment.Equals("n"))
            comment = null;

        var withdraw = new Withdraw(selectedAccount.AccountNo, amount, comment);

        if (!selectedAccount.Debit(amount))
        {
            "Transaction failed.".ConsoleColorRed();
            return;
        }

        selectedAccount.AddTransaction(withdraw);

        Console.WriteLine($"Withdraw of {amount:C} successful, account balance is now {selectedAccount.Balance:C}");
        Console.WriteLine();
    }

    private static void Transfer()
    {
        Console.WriteLine();
        var accountCounter = 1;
        Console.WriteLine("--- Transfer ---");
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

        if (!int.TryParse("Enter destination account number: ".ReadInput(), out var destinationAccountNumber))
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
            $"Available Balance: {selectedAccount.AvailableBalance:C}");

        if (decimal.TryParse("Enter amount: ".ReadInput(), out var amount) && amount <= 0)
        {
            "Amount cannot be negative.".ConsoleColorRed();
            return;
        }

        var comment = "Enter comment (n to quit, max length 30): ".ReadInput();
        if (string.IsNullOrWhiteSpace(comment) || comment.Equals("n"))
            comment = null;

        var transfer = new Transfer(selectedAccount.AccountNo, destinationAccountNumber, amount, comment);

        if (!selectedAccount.Debit(amount))
        {
            "Transaction failed.".ConsoleColorRed();
            return;
        }

        selectedAccount.AddTransaction(transfer);

        Console.WriteLine($"Transfer of {amount:C} successful, account balance is now {selectedAccount.Balance:C}");
        Console.WriteLine();
    }

    private static void MyStatement()
    {
        Console.WriteLine();
        var accountCounter = 1;
        Console.WriteLine("--- Statements ---");
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
            $"{selectedAccount.AccountNo}, " +
            $"Balance: {selectedAccount.Balance:C}, " +
            $"Available Balance: {selectedAccount.AvailableBalance:C}");

        Console.WriteLine();


        Console.WriteLine(
            $"{"ID",-20}{"Type",-20}{"Account Number",-20}{"Destination",-20}{"Time",-20}{"Comment",-20}");


        var quit = false;
        var start = 0;
        var count = 4;
        var length = selectedAccount.Transactions.Count;
        var pages = (selectedAccount.Transactions.Count / 4) + 1;
        var currentPage = 1;
        var separator = new string('-', 20);
        do
        {
            selectedAccount.Transactions.GetRange(start, count).ForEach(transaction =>
            {
                
                Console.WriteLine(
                    $"{transaction.TransactionId,-20}{transaction.TransactionType,-20}{transaction.AccountNumber,-20}" +
                    $"{transaction.DestinationAccountNumber,-20}{transaction.Amount,-20:C}{transaction.TransactionTimeUtc.ToLocalTime().ToString("dd/MM/yyyy HH:mm tt"),-20}" +
                    $"{transaction.Comment,-20}");
            });

            if (count < 4)
            {
                for (var i = 0; i < 4 - count; i++)
                {
                    Console.WriteLine(
                        $"{separator,-20}{separator,-20}{separator,-20}" +
                        $"{separator,-20}{separator,-20}{separator,-20}" +
                        $"{separator,-20}");
                }
            }

            Console.WriteLine($"{currentPage} of {pages}");
            Console.WriteLine("Options: n (next page) | p (previous page | q (quit))");
            switch ("Enter an option: ".ReadInput())
            {
                case "n":
                    start += 4;
                    if (start + count >= length)
                        count = length % 4;
                    currentPage++;
                    Console.SetCursorPosition(0, Console.CursorTop - 7);
                    break;
                case "p":
                    start -= 4;
                    count = 4;
                    currentPage--;
                    Console.SetCursorPosition(0, Console.CursorTop - 7);
                    break;
                case "q":
                    quit = true;
                    break;
                default:
                    "Invalid input".ConsoleColorRed();
                    return;
            }
        } while (!quit);

        //
        // var transactionsCount = selectedAccount.Transactions.Count;
        // for (var i = 0; i < transactionsCount; i++)
        // {
        //     var cursorPosition = Console.GetCursorPosition();
        //     Console.WriteLine(
        //         $"{selectedAccount.Transactions[i].TransactionId,-20}{selectedAccount.Transactions[i].TransactionType,-20}{selectedAccount.Transactions[i].AccountNumber,-20}" +
        //         $"{selectedAccount.Transactions[i].DestinationAccountNumber,-20}{selectedAccount.Transactions[i].Amount,-20}{selectedAccount.Transactions[i].TransactionTimeUtc.ToLocalTime().ToString("dd/MM/yyyy HH:mm tt"),-20}" +
        //         $"{selectedAccount.Transactions[i].Comment,-20}");
        //
        //     if (4 * i + 3 == i)
        //     {
        //         Console.WriteLine($"Page 1 of 4");
        //         Console.WriteLine("Options: n (next page) | p (previous page | q (quit))");
        //         switch ("Enter an option: ".ReadInput())
        //         {
        //             case "n":
        //                 if (i != transactionsCount && i < transactionsCount - 4)
        //                     Console.SetCursorPosition(cursorPosition.Left, cursorPosition.Top);
        //                 break;
        //             case "p":
        //                 if (i != selectedAccount.Transactions.Count && i != 0)
        //                 {
        //                     Console.SetCursorPosition(cursorPosition.Left, cursorPosition.Top);
        //                     i -= 4;
        //                 }
        //
        //                 break;
        //             case "q":
        //                 return;
        //             default:
        //                 "Invalid input".ConsoleColorRed();
        //                 return;
        //         }
        //     }
        //     if(i == transactionsCount)
        // }

        Console.WriteLine();
    }

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