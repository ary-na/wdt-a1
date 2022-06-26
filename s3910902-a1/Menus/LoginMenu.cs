using System.Text;
using s3910902_a1.Mangers;

namespace s3910902_a1.Menus;

// Code sourced and adapted from:
// https://docs.microsoft.com/en-us/dotnet/standard/base-types/stringbuilder
// https://github.com/Job79/SimpleHashing
// https://docs.microsoft.com/en-us/dotnet/api/system.string.isnullorwhitespace?view=net-6.0
// https://stackoverflow.com/questions/17215045/best-way-to-remove-the-last-character-from-a-string-built-with-stringbuilder
// https://stackoverflow.com/questions/5701163/removing-a-character-from-my-stringbuilder#5701216
// https://stackoverflow.com/questions/20409026/how-to-remove-char-from-console-window-command-console-write-b-didnt
// https://docs.microsoft.com/en-us/dotnet/api/system.console.readkey?view=net-6.0

public static class LoginMenu
{
    public static void Run(string connectionString)
    {
        var loginManager = new LoginManager(connectionString);
        bool isValid;
        do
        {
            Console.Write("Enter Login ID: ");
            var loginId = Console.ReadLine();

            Console.Write("Enter Password: ");
            var password = ReadPassword();

            isValid = loginManager.VerifyLogin(loginId?.Trim(), password.Trim());

            Console.WriteLine();

            if (!isValid)
                Console.WriteLine("Invalid Login ID and/or Password, try again.");
        } while (!isValid);

        var customerManager = new CustomerManager(connectionString, loginManager);

        foreach (var x in customerManager.Customer.Accounts)
        {
            Console.WriteLine(x.Balance);
        }

        if (isValid)
            MainMenu.Run();
    }

    private static string ReadPassword()
    {
        StringBuilder password = new();
        ConsoleKeyInfo readPassword = new();
        while (readPassword.Key != ConsoleKey.Enter)
        {
            readPassword = Console.ReadKey(true);
            password.Append(readPassword.KeyChar);

            // Remove password character on backspace
            if (readPassword.Key is ConsoleKey.Backspace && password.Length > 1)
            {
                Console.Write("\b \b");
                password.Remove(password.Length - 1, 1);
                password.Length--;
            }
            else
                Console.Write("*");
        }

        // Remove enter character
        Console.WriteLine("\b \b");
        password.Length--;
        return password.ToString();
    }
}