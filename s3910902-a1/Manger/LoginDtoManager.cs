using Microsoft.Data.SqlClient;
using s3910902_a1.Dto;

namespace s3910902_a1.Manger;

public class LoginDtoManager
{
    private readonly string _connectionString;

    public LoginDtoManager(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public void InsertLogin(LoginDto loginDto)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText =
            @"insert into Login (LoginID, CustomerID, PasswordHash)
            values (@loginId, @customerId, @passwordHash)";
        
        command.Parameters.AddWithValue("loginId", loginDto.LoginId);
        command.Parameters.AddWithValue("customerId", loginDto.CustomerId);
        command.Parameters.AddWithValue("passwordHash", loginDto.PasswordHash);

        command.ExecuteNonQuery();
    }
}