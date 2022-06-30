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
            switch (ReadKeyInput())
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
                    Statements();
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
                    Console.WriteLine("Please select a valid menu option.\n");
                    break;
            }
        } while (!exit);
    }

    // Code sourced and adapted from:
    // Week 1 Lectorial - Person.cs
    // https://rmit.instructure.com/courses/102750/files/25011410?wrap=1

    // Code sourced and adapted from:
    // https://docs.microsoft.com/en-us/dotnet/api/system.string.isnullorwhitespace?view=net-6.0
    // https://docs.microsoft.com/en-us/dotnet/api/system.datetime.utcnow?view=net-6.0
    // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/conditional-operator
    // https://docs.microsoft.com/en-us/dotnet/api/system.int32.tryparse?view=net-6.0

    // Menu method Deposit
    private static void Deposit()
    {
        PrintAccounts(nameof(Deposit));

        // Read and validate inputs
        var selectedAccountInput = "\nSelect an account: ".ReadInput();
        if (!selectedAccountInput.IsValidAccount())
            return;

        var selectedAccount = int.Parse(selectedAccountInput) is 1 ? _customerManager.Customer.Accounts[0] : _customerManager.Customer.Accounts[1];
        Console.WriteLine($"\n{selectedAccount.AccountType} {selectedAccount.AccountNo} Balance: {selectedAccount.Balance:C}" +
                          $" Available Balance: {selectedAccount.AvailableBalance:C}");

        var enteredAmount = "\nEnter amount: ".ReadInput();
        if (!enteredAmount.IsValidAmount())
            return;

        var comment = "Enter comment (n to quit, max length 30): ".ReadInput();
        if (!comment.IsValidComment())
            return;
        comment = comment.IsNullComment() ? null : comment;

        // Create deposit
        var deposit = TransactionFactory.Create(TransactionType.Deposit, selectedAccount.AccountNo,
            decimal.Parse(enteredAmount), comment);

        // Add deposit transaction
        selectedAccount.Credit(deposit.Amount);
        selectedAccount.AddTransaction(deposit);

        Console.WriteLine($"\nDeposit of {deposit.Amount:C} successful, account balance is now {selectedAccount.Balance:C}\n");
    }

    // Menu method Withdraw
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
        var withdraw = TransactionFactory.Create(TransactionType.Withdraw, selectedAccount.AccountNo,
            decimal.Parse(enteredAmount), comment);

        // Add withdraw transaction
        if (!selectedAccount.Debit(withdraw.Amount))
        {
            "Transaction failed.".ConsoleColorRed();
            return;
        }
        selectedAccount.AddTransaction(withdraw);

        Console.WriteLine($"\nWithdraw of {withdraw.Amount:C} successful, account balance is now {selectedAccount.Balance:C}\n");
    }

    // Menu method Transfer
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
        var transfer = TransactionFactory.Create(TransactionType.Transfer, selectedAccount.AccountNo,
            decimal.Parse(enteredAmount), comment, int.Parse(enteredDestinationAccount));

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

    // Menu method Statements
    private static void Statements()
    {
        PrintAccounts(nameof(Statements));

        // Read and validate inputs
        var selectedAccountInput = "\nSelect an account: ".ReadInput();
        if (!selectedAccountInput.IsValidAccount())
            return;

        var selectedAccount = int.Parse(selectedAccountInput) is 1 ? _customerManager.Customer.Accounts[0] : _customerManager.Customer.Accounts[1];
        Console.WriteLine($"\n{selectedAccount.AccountType} {selectedAccount.AccountNo} Balance: {selectedAccount.Balance:C} " +
                          $"Available Balance: {selectedAccount.AvailableBalance:C}");

        Console.WriteLine($"{"ID",-20}{"Type",-20}{"Account Number",-20}{"Destination",-20}{"Amount",-20}{"Time",-20}{"Comment",-20}");
        ProcessStatement(selectedAccount.Transactions);
    }

    // Code sourced and adapted from:
    // https://docs.microsoft.com/en-us/dotnet/api/system.console.clear?view=net-6.0
    
    // Menu method Logout
    private static void Logout()
    {
        Console.Clear();
        _customerManager = null;
        LoginMenu.Run(new ModelManger());
    }

    // Menu method Exit
    private static void Exit() => Console.WriteLine("Program ending!");

    // Code sourced and adapted from:
    // https://www.geeksforgeeks.org/different-methods-to-read-a-character-in-c-sharp/

    // Read user input as character
    private static char ReadKeyInput()
    {
        var input = Console.ReadKey().KeyChar;
        Console.WriteLine();
        return input;
    }

    // Code sourced and adapted from:
    // Week 3 Lectorial - FormatStrings.cs
    // Week 3 Lectorial - PatternMatching.cs
    // https://rmit.instructure.com/courses/102750/files/24463725?wrap=1

    // Print menu
    private static void PrintMenu()
    {
        var separator = new string('-', 29);
        Console.WriteLine($"\n{separator}");
        Console.WriteLine("Most Common Bank of Australia");
        Console.WriteLine($"{separator}\n");
        Console.WriteLine($"--- {_customerManager?.Customer?.Name} ---\n");
        Console.WriteLine("[1] Deposit");
        Console.WriteLine("[2] Withdraw");
        Console.WriteLine("[3] Transfer");
        Console.WriteLine("[4] My Statement");
        Console.WriteLine("[5] Logout");
        Console.WriteLine("[6] Exit\n");
        Console.Write("Enter an option: ");
    }

    // Print accounts
    private static void PrintAccounts(string transactionType)
    {
        var accounts = _customerManager?.Customer?.Accounts;
        Console.WriteLine($"\n--- {transactionType} ---\n");

        for (var i = 0; i < accounts?.Length; i++)
            Console.WriteLine($"{i + 1}. {accounts?[i].AccountType,-20}{accounts?[i].AccountNo,-20}{accounts?[i].Balance,-20:C}");
    }

    // Process statements as pages
    private static void ProcessStatement(IEnumerable<ITransaction> selectedAccount)
    {
        var quit = false;
        var length = selectedAccount.Count();
        // Start and count for range
        var start = 0;
        var count = length < 4 ? length : 4;
        // page count
        var pages = length % 4 == 0 ? length / 4 : (length / 4) + 1;
        var currentPage = 1;

        do
        {
            PrintStatement(selectedAccount, start, count);
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
                    // Reset cursor position
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
    }

    // Code sourced and adapted from:
    // https://docs.microsoft.com/en-us/visualstudio/ide/reference/invert-if-statement?view=vs-2022

    // Print statement
    private static void PrintStatement(IEnumerable<ITransaction> accountTransactions, int start, int count)
    {
        // Order transactions by date and time
        var transactions = accountTransactions.OrderByDescending(x => x.TransactionTimeUtc.Date)
            .ThenByDescending(x => x.TransactionTimeUtc.TimeOfDay).ToList();

        // Print transactions by range
        transactions.GetRange(start, count).ToList().ForEach(x =>
        {
            Console.WriteLine($"{x.TransactionId,-20}{x.TransactionType,-20}{x.AccountNumber,-20}" +
                              $"{x.DestinationAccountNumber,-20}{x.Amount,-20:C}" +
                              $"{x.TransactionTimeUtc.ToLocalTime().ToString("dd/MM/yyyy HH:mm tt"),-20}" +
                              $"{x.Comment,-20}");
        });

        // fill empty lines 
        if (count >= 4) return;
        for (var i = 0; i < 4 - count; i++)
            Console.WriteLine($"{"",-20}{"",-20}{"",-20}{"",-20}{"",-20}{"",-20}{"",-20}");
    }
}