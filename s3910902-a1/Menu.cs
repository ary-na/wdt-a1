namespace s3910902_a1;

public static class Menu
{
    // Code sourced and adapted from:
    // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/statements/selection-statements#code-try-0
    // https://www.geeksforgeeks.org/switch-statement-in-c-sharp/

    // Run menu
    public static void Run()
    {
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
                    break;
                case '6':
                    Exit();
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Please select a valid menu option.");
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
        var separator = new string('-', 40);
        Console.WriteLine(separator);
        Console.WriteLine("Most Common Bank of Australia");
        Console.WriteLine("Please select a number from the menu:");
        Console.WriteLine("[1] Deposit");
        Console.WriteLine("[2] Withdraw");
        Console.WriteLine("[3] Transfer");
        Console.WriteLine("[4] My Statement");
        Console.WriteLine("[5] Logout");
        Console.WriteLine("[6] Exit");
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
    private static void Deposit() => Console.WriteLine("Deposit");
    private static void Withdraw() => Console.WriteLine("Withdraw");
    private static void Transfer() => Console.WriteLine("Transfer");
    private static void MyStatement() => Console.WriteLine("MyStatement");
    private static void Logout() => Console.WriteLine("Logout");
    private static void Exit() => Console.WriteLine("Exit");
}