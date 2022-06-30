using System.Data;
using Microsoft.Data.SqlClient;
using s3910902_a1.Models;
using SimpleHashing;
using Utilities.DTOs;
using Utilities.ExtensionMethods;

namespace s3910902_a1.Mangers;

// Code sourced and adapted from:
// Week 3 Tutorial - PersonManager.cs
// https://rmit.instructure.com/courses/102750/files/24402824?wrap=1

public class LoginManager
{
    private readonly string _connectionString;
    public Login? Login { get; private set; }
    public List<int> Logins { get; }

    public LoginManager(string connectionString)
    {
        _connectionString = connectionString;
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = "select CustomerID from [Login]";
        Logins = command.GetDataTable().Select().Select(x => x.Field<int>("CustomerID")).ToList();
    }

    public bool VerifyLogin(string? loginId, string password)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = "select * from [Login] where LoginID = @loginId";
        command.Parameters.AddWithValue("loginId", loginId);

        // Code sourced and adapted from:
        // https://docs.microsoft.com/en-us/dotnet/csharp/linq/handle-null-values-in-query-expressions
        // https://www.jetbrains.com/help/resharper/InvertIf.html
        // https://stackoverflow.com/questions/40415020/datatable-nullreferenceexception

        if (command.GetDataTable().Rows.Count is 0) return false;

        Login = command.GetDataTable().Select().Select(CreateLogin).Single();

        // Code sourced and adapted from:
        // https://stackoverflow.com/questions/55005406/one-line-if-else-in-c-sharp
        // https://stackoverflow.com/questions/14621907/simplify-conditional-ternary-expression

        var passwordHash = PBKDF2.Verify(Login.PasswordHash, password);
        return Login.PasswordHash != null && passwordHash;
    }

    public async Task InsertLogin(LoginDto loginDto)
    {
        await using var connection = new SqlConnection(_connectionString);
        connection.Open();

        await using var command = connection.CreateCommand();
        command.CommandText =
            @"insert into [Login] (LoginID, CustomerID, PasswordHash)
            values (@loginId, @customerId, @passwordHash)";

        command.Parameters.AddWithValue("loginId", loginDto.LoginId);
        command.Parameters.AddWithValue("customerId", loginDto.CustomerId);
        command.Parameters.AddWithValue("passwordHash", loginDto.PasswordHash);

        await Task.WhenAny(command.ExecuteNonQueryAsync());
    }

    private static Login CreateLogin(DataRow dataRow)
    {
        return new Login
        {
            LoginId = dataRow.Field<string>("LoginID"),
            CustomerId = dataRow.Field<int>("CustomerID"),
            PasswordHash = dataRow.Field<string>("PasswordHash")
        };
    }
}