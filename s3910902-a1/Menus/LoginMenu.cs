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
    private static string? _loginId;
    private static string? _passwordHash;
    private static readonly StringBuilder Password = new();
    public static void Run(string connectionString)
    {
        Console.Write("Enter Login ID: ");
        _loginId = Console.ReadLine();

        Console.Write("Enter Password: ");
        ConsoleKeyInfo readPassword;
        
        do
        {
            readPassword = Console.ReadKey(true);
            Password.Append(readPassword.KeyChar);

            if (readPassword.Key == ConsoleKey.Backspace && Password.Length > 1)
            {
                Console.Write("\b \b");
                Password.Remove(Password.Length - 1, 1);
                Password.Length--;
            }
            else if(readPassword.Key != ConsoleKey.Enter)
                Console.Write("*");

        } while (readPassword.Key != ConsoleKey.Enter);
        Password.Length--;

        var loginManager = new LoginManager(connectionString);
        var loggedIn = loginManager.VerifyLogin(_loginId, Password.ToString());
        Console.WriteLine();
        Console.WriteLine(loggedIn);
    }
}