using System.Data;
using Microsoft.Data.SqlClient;
using s3910902_a1.Dto;
using s3910902_a1.Utilities;
using SimpleHashing;

namespace s3910902_a1.Manger;

public class LoginManager
{
    private readonly string _connectionString;

    public LoginManager(string connectionString)
    {
        _connectionString = connectionString;
        using var connection = new SqlConnection(_connectionString);
        using var command = connection.CreateCommand();
        command.CommandText = "select * from [Login] where LoginID = @loginId and PasswordHash = @passwordHash";
    }

    public bool VerifyLogin(string? loginId, string password)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText =
            "select PasswordHash from [Login] where LoginID = @loginId";

        command.Parameters.AddWithValue("loginId", loginId);

        string passwordHash = command.GetDataTable().Select().Select(x => x).ToString();
        
        return PBKDF2.Verify(passwordHash, password);
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