using Microsoft.Data.SqlClient;
using s3910902_a1.Dto;

namespace s3910902_a1.Manger;

// Code sourced and adapted from:
// Week 3 Tutorial - PersonManager.cs
// https://rmit.instructure.com/courses/102750/files/24402824?wrap=1

public class AccountManager
{
    private readonly string _connectionString;

    public AccountManager(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void InsertAccount(AccountDto accountDto)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText =
            @"insert into [Account] (AccountNumber, AccountType, CustomerID, Balance)
            values (@accountNumber, @accountType, @customerId, @balance)";
        
        command.Parameters.AddWithValue("accountNumber", accountDto.AccountNumber);
        command.Parameters.AddWithValue("accountType", accountDto.AccountType);
        command.Parameters.AddWithValue("customerId", accountDto.CustomerId);
        command.Parameters.AddWithValue("balance", accountDto.Balance);

        command.ExecuteNonQuery();
    }
}