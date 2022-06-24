using System.Data;
using Microsoft.Data.SqlClient;
using s3910902_a1.Dto;
using s3910902_a1.Models;
using s3910902_a1.Utilities;
using SimpleHashing;

namespace s3910902_a1.Mangers;

// Code sourced and adapted from:
// Week 3 Tutorial - PersonManager.cs
// https://rmit.instructure.com/courses/102750/files/24402824?wrap=1

public class LoginManager
{
    private readonly string _connectionString;
    public Login Login { get; private set; }
    public bool ValidLogin { get; private set; }

    public LoginManager(string connectionString)
    {
        _connectionString = connectionString;
        
        
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText =
            "select * from [Login] where LoginID = @loginId";

        command.Parameters.AddWithValue("loginId", 12345678);

        Login = command.GetDataTable().Select().Select(x => new Login
        {
            LoginId = x.Field<string>("LoginID"),
            CustomerId = x.Field<int>("CustomerID"),
            PasswordHash = x.Field<string>("PasswordHash")
        }).First();
        
        // using var connection = new SqlConnection(_connectionString);
        // using var command = connection.CreateCommand();
        // command.CommandText = "select * from [Login] where LoginID = @loginId and PasswordHash = @passwordHash";
    }

    // Code sourced and adapted from:
    // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/using-constructors
    public LoginManager(string connectionString, string? loginId, string password) : this(connectionString)
    {
        
    }

    public bool VerifyLogin(string? loginId, string password)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText =
            "select * from [Login] where LoginID = @loginId";

        command.Parameters.AddWithValue("loginId", loginId);

        Login = command.GetDataTable().Select().Select(x => new Login
        {
            LoginId = x.Field<string>("LoginID"),
            CustomerId = x.Field<int>("CustomerID"),
            PasswordHash = x.Field<string>("PasswordHash")
        }).First();

        // Code sourced and adapted from:
        // https://stackoverflow.com/questions/55005406/one-line-if-else-in-c-sharp
        // https://stackoverflow.com/questions/14621907/simplify-conditional-ternary-expression
        
        var passwordHash = PBKDF2.Verify(Login.PasswordHash, password);
        ValidLogin = passwordHash;
        return Login.PasswordHash != null && passwordHash;
    }

    public void InsertLogin(LoginDto loginDto)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText =
            @"insert into [Login] (LoginID, CustomerID, PasswordHash)
            values (@loginId, @customerId, @passwordHash)";

        command.Parameters.AddWithValue("loginId", loginDto.LoginId);
        command.Parameters.AddWithValue("customerId", loginDto.CustomerId);
        command.Parameters.AddWithValue("passwordHash", loginDto.PasswordHash);

        command.ExecuteNonQuery();
    }
}