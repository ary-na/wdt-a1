using s3910902_a1.Factories;
using s3910902_a1.Mangers;
using s3910902_a1.Models;
using s3910902_a1.Validation;
using Utilities.ExtensionMethods;

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
    // Week 3 Lectorial - PatternMatching.cs
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

    private static void PrintAccounts(string transactionType)
    {
        var accounts = _customerManager?.Customer?.Accounts;
        Console.WriteLine($"\n--- {transactionType} ---\n");

        for (var i = 0; i < accounts?.Length; i++)
            Console.WriteLine(
                $"{i + 1}. {accounts?[i].AccountType,-20}{accounts?[i].AccountNo,-20}{accounts?[i].Balance,-20:C}");
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
        PrintAccounts(nameof(Deposit));

        // Read and validate inputs
        var selectedAccountInput = "\nSelect an account: ".ReadInput();
        if (!selectedAccountInput.IsValidAccount())
            return;

        var selectedAccount = int.Parse(selectedAccountInput) is 1 ? _customerManager.Customer.Accounts[0] : _customerManager.Customer.Accounts[1];
        Console.WriteLine($"\n{selectedAccount.AccountType} {selectedAccount.AccountNo} Balance: {selectedAccount.Balance:C} " +
                          $"Available Balance: {selectedAccount.AvailableBalance:C}");

        var enteredAmount = "\nEnter amount: ".ReadInput();
        if (!enteredAmount.IsValidAmount())
            return;
        
        var comment = "Enter comment (n to quit, max length 30): ".ReadInput();
        if (!comment.IsValidComment())
            return;
        comment = comment.IsNullComment() ? null : comment;

        // Create deposit
        var deposit = TransactionFactory.Create(TransactionType.Deposit, selectedAccount.AccountNo, decimal.Parse(enteredAmount), comment);
        
        // Add deposit transaction
        selectedAccount.Credit(deposit.Amount);
        selectedAccount.AddTransaction(deposit);

        Console.WriteLine($"\nDeposit of {deposit.Amount:C} successful, account balance is now {selectedAccount.Balance:C}\n");
    }

    private static void Withdraw()
    {
        PrintAccounts(nameof(Withdraw));
        
        // Read and validate inputs
        var selectedAccountInput = "\nSelect an account: ".ReadInput();
        if (!selectedAccountInput.IsValidAccount())
            return;
       
        var selectedAccount = int.Parse(selectedAccountInput) is 1 ? _customerManager.Customer.Accounts[0] : _customerManager.Customer.Accounts[1];
        Console.WriteLine($"\n{selectedAccount.AccountType} {selectedAccount.AccountNo} Balance: {selectedAccount.Balance:C} " +
                          $"Available Balance: {selectedAccount.AvailableBalance:C}");

        var enteredAmount = "\nEnter amount: ".ReadInput();
        if (!enteredAmount.IsValidAmount())
            return;
        
        var comment = "Enter comment (n to quit, max length 30): ".ReadInput();
        if (!comment.IsValidComment())
            return;
        comment = comment.IsNullComment() ? null : comment;

        // Create withdraw
        var withdraw = TransactionFactory.Create(TransactionType.Withdraw, selectedAccount.AccountNo, decimal.Parse(enteredAmount), comment);

        // Add withdraw transaction
        if (!selectedAccount.Debit(withdraw.Amount))
        {
            "Transaction failed.".ConsoleColorRed();
            return;
        }
        selectedAccount.AddTransaction(withdraw);

        Console.WriteLine($"\nWithdraw of {withdraw.Amount:C} successful, account balance is now {selectedAccount.Balance:C}\n");
    }

    private static void Transfer()
    {
        PrintAccounts(nameof(Transfer));
        
        // Read and validate inputs
        var selectedAccountInput = "\nSelect an account: ".ReadInput();
        if (!selectedAccountInput.IsValidAccount())
            return;
        
        var selectedAccount = int.Parse(selectedAccountInput) is 1 ? _customerManager.Customer.Accounts[0] : _customerManager.Customer.Accounts[1];
        
        var enteredDestinationAccount = "\nEnter destination account number: ".ReadInput();
        if (!enteredDestinationAccount.IsValidDestinationAccount(selectedAccount.AccountNo))
            return;
        
        Console.WriteLine($"\n{selectedAccount.AccountType} {selectedAccount.AccountNo} Balance: {selectedAccount.Balance:C} " +
                          $"Available Balance: {selectedAccount.AvailableBalance:C}");

        var enteredAmount = "\nEnter amount: ".ReadInput();
        if (!enteredAmount.IsValidAmount())
            return;
        
        var comment = "Enter comment (n to quit, max length 30): ".ReadInput();
        if (!comment.IsValidComment())
            return;
        comment = comment.IsNullComment() ? null : comment;
        
        // Create transfer
        var transfer = TransactionFactory.Create(TransactionType.Transfer, selectedAccount.AccountNo, decimal.Parse(enteredAmount), comment, int.Parse(enteredDestinationAccount));

        // Add transfer transaction
        if (!selectedAccount.Debit(transfer.Amount))
        {
            "Transaction failed.".ConsoleColorRed();
            return;
        }
        selectedAccount.AddTransaction(transfer);

        Console.WriteLine($"\nTransfer of {transfer.Amount:C} successful, account balance is now {selectedAccount.Balance:C}\n");
    }

    // Code sourced and adapted from:
    // https://stackoverflow.com/questions/30910046/only-clear-part-of-console-window
    // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-8.0/ranges
    // https://stackoverflow.com/questions/200574/linq-equivalent-of-foreach-for-ienumerablet
    // https://stackoverflow.com/questions/24193898/how-do-i-convert-foreach-statement-into-linq-expression
    // https://stackoverflow.com/questions/14848275/how-to-get-remainder-and-mod-by-dividing-using-c-sharp
    // https://stackoverflow.com/questions/8985131/how-to-write-in-current-line-of-console
    // https://stackoverflow.com/questions/40125217/linq-and-order-by-a-date-field
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
            $"{"ID",-20}{"Type",-20}{"Account Number",-20}{"Destination",-20}{"Amount",-20}{"Time",-20}{"Comment",-20}");


        var quit = false;
        var length = selectedAccount.Transactions.Count;
        var start = 0;
        var count = length < 4 ? length : 4;
        var pages = selectedAccount.Transactions.Count % 4 == 0 ? length / 4 : (length / 4) + 1;
        var currentPage = 1;
        var separator = new string('-', 20);

        var selectedAccount1 = selectedAccount.Transactions
            .OrderByDescending(x => x.TransactionTimeUtc.Date)
            .ThenByDescending(x => x.TransactionTimeUtc.TimeOfDay)
            .ToList();

        do
        {
            selectedAccount1.GetRange(start, count)
                .ToList().ForEach(transaction =>
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
                        $"{"",-20}{"",-20}{"",-20}" +
                        $"{"",-20}{"",-20}{"",-20}" +
                        $"{"",-20}");
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
                    if (start >= length)
                    {
                        start = 0;
                        count = length < 4 ? length : 4;
                        currentPage = 0;
                    }

                    currentPage++;
                    Console.SetCursorPosition(0, Console.CursorTop - 7);
                    break;
                case "p":
                    if (start > 0)
                    {
                        start -= 4;
                        count = 4;
                        currentPage--;
                    }

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