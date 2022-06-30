using System.Data;
using Microsoft.Data.SqlClient;
using s3910902_a1.Models;
using Utilities.DTOs;
using Utilities.ExtensionMethods;

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
        command.CommandText = "select * from [Account] where CustomerID = @customerId";
        command.Parameters.AddWithValue("customerId", customerId);

        // Code sourced and adapted from:
        // https://stackoverflow.com/questions/3841990/are-there-any-benefits-to-using-a-c-sharp-method-group-if-available
        // https://www.jetbrains.com/help/rider/ConvertClosureToMethodGroup.html

        return command.GetDataTable().Select().Select(CreateAccount).ToArray();
    }

    public static async Task InsertAccount(SqlConnection connection, AccountDto accountDto)
    {
        await using var command = connection.CreateCommand();
        command.CommandText =
            @"insert into [Account] (AccountNumber, AccountType, CustomerID, Balance)
            values (@accountNumber, @accountType, @customerId, @balance)";

        command.Parameters.AddWithValue("accountNumber", accountDto.AccountNumber);
        command.Parameters.AddWithValue("accountType", accountDto.AccountType);
        command.Parameters.AddWithValue("customerId", accountDto.CustomerId);
        command.Parameters.AddWithValue("balance", accountDto.Balance);

        await Task.WhenAny(command.ExecuteNonQueryAsync());
    }

    // Code sourced and adapted from:
    // https://dotnetcoretutorials.com/2019/10/15/the-factory-pattern-in-net-core/
    // https://mahedee.net/factory-design-pattern/

    private IAccount CreateAccount(DataRow dataRow)
    {
        var transactionManager = new TransactionManager(_connectionString);
        return dataRow.Field<string>("AccountType") switch
        {
            "S" => new SavingAccount
            {
                AccountNo = dataRow.Field<int>("AccountNumber"),
                AccountType = AccountType.Savings,
                CustomerId = dataRow.Field<int>("CustomerID"),
                Balance = dataRow.Field<decimal>("Balance"),
                AvailableBalance = dataRow.Field<decimal>("Balance"),
                Transactions = transactionManager.GetTransactions(dataRow.Field<int>("AccountNumber"))
            },
            "C" => new CheckingAccount
            {
                AccountNo = dataRow.Field<int>("AccountNumber"),
                AccountType = AccountType.Checking,
                CustomerId = dataRow.Field<int>("CustomerID"),
                Balance = dataRow.Field<decimal>("Balance"),
                AvailableBalance = dataRow.Field<decimal>("Balance") - 300M,
                Transactions = transactionManager.GetTransactions(dataRow.Field<int>("AccountNumber"))
            },
            _ => throw new NullReferenceException()
        };
    }
}