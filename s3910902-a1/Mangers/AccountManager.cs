using System.Data;
using Microsoft.Data.SqlClient;
using s3910902_a1.Dto;
using s3910902_a1.Models;
using s3910902_a1.Utilities;

namespace s3910902_a1.Mangers;

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

    public IAccount[] GetAccounts(int customerId)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = connection.CreateCommand();
        command.CommandText = "select * from Account where CustomerID = @customerId";
        command.Parameters.AddWithValue("customerId", customerId);

        var transactionManager = new TransactionManager(_connectionString);

        var savingAccount = command.GetDataTable().Select()
            .Where(x => x.Field<char>("AccountType") == 'S')
            .Select(x => new SavingAccount
            {
                AccountNo = x.Field<int>("AccountNumber"),
                AccountType = x.Field<char>("AccountType"),
                Balance = x.Field<decimal>("Balance"),
                Transactions = transactionManager.GetTransactions(x.Field<int>("AccountNumber"))
            }).First();

        var checkingAccount = command.GetDataTable().Select()
            .Where(x => x.Field<char>("AccountType") == 'C')
            .Select(x => new SavingAccount
            {
                AccountNo = x.Field<int>("AccountNumber"),
                AccountType = x.Field<char>("AccountType"),
                Balance = x.Field<decimal>("Balance"),
                Transactions = transactionManager.GetTransactions(x.Field<int>("AccountNumber"))
            }).First();

        return new IAccount[]
        {
            savingAccount,
            checkingAccount
        };
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