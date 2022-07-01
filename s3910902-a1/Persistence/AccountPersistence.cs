using System.Data;
using Microsoft.Data.SqlClient;
using s3910902_a1.Mangers;
using s3910902_a1.Models;
using Utilities.ExtensionMethods;

namespace s3910902_a1.Persistence;

// Code sourced and adapted from:
// Week 3 Lectorial - AnimalPersistence.cs
// https://rmit.instructure.com/courses/102750/files/24463725?wrap=1

public class AccountPersistence : IAccountPersistence
{
    private static readonly ModelManger ModelManger = new();

    // Code sourced and adapted from:
    // https://blog.jetbrains.com/dotnet/2019/05/14/switch-expressions-pattern-based-usings-look-new-language-features-c-8/
    // https://docs.microsoft.com/en-us/azure/mysql/flexible-server/connect-csharp
    // https://stackoverflow.com/questions/16016023/what-is-the-use-of-a-persistence-layer-in-any-application
    // https://stackoverflow.com/questions/20160928/how-to-count-the-number-of-rows-from-sql-table-in-c
    // https://docs.microsoft.com/en-us/answers/questions/296142/c-mysql-check-if-value-exists-issue.html

    public ITransaction InsertTransaction(ITransaction transaction)
    {
        // Insert transaction
        using var connection = new SqlConnection(ModelManger.ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText =
            @"insert into [Transaction] (TransactionType, AccountNumber, DestinationAccountNumber, Amount, Comment, TransactionTimeUtc)
            values (@transactionType, @accountNumber, @destinationAccountNumber, @amount, @comment, @transactionTimeUtc)";

        var transactionType = transaction.TransactionType switch
        {
            TransactionType.Deposit => "D",
            TransactionType.Withdraw => "W",
            TransactionType.Transfer => "T",
            TransactionType.Service => "S",
            _ => throw new NullReferenceException()
        };

        command.Parameters.AddWithValue("transactionType", transactionType);
        command.Parameters.AddWithValue("accountNumber", transaction.AccountNumber);
        command.Parameters.AddWithValue("destinationAccountNumber", transaction.DestinationAccountNumber.GetObjectOrDbNull());
        command.Parameters.AddWithValue("amount", transaction.Amount);
        command.Parameters.AddWithValue("comment", transaction.Comment.GetObjectOrDbNull());
        command.Parameters.AddWithValue("transactionTimeUtc", transaction.TransactionTimeUtc);

        command.ExecuteNonQuery();

        // Return transaction
        return transaction;
    }

    public decimal UpdateBalance(int accountNumber, decimal balance)
    {
        // Update balance
        using var connection = new SqlConnection(ModelManger.ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = $"update [Account] set Balance = @balance where AccountNumber = @accountNumber";
        command.Parameters.AddWithValue("balance", balance);
        command.Parameters.AddWithValue("accountNumber", accountNumber);

        command.ExecuteNonQuery();

        // Return balance
        return balance;
    }

    public decimal GetBalance(int accountNumber)
    {
        // Get balance
        using var connection = new SqlConnection(ModelManger.ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = $"select Balance from [Account] where AccountNumber = @accountNumber";
        command.Parameters.AddWithValue("accountNumber", accountNumber);

        command.ExecuteNonQuery();

        // Return balance
        return command.GetDataTable().Select().Select(x => x.Field<decimal>("Balance")).Single();
    }

    public int CountTransactions(int accountNumber)
    {
        // Count transactions
        using var connection = new SqlConnection(ModelManger.ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = 
            $@"select count(*) from [Transaction] where AccountNumber = @accountNumber and 
            (TransactionType = @transactionTypeWithdraw or TransactionType = @transactionTypeTransfer)";
        command.Parameters.AddWithValue("accountNumber", accountNumber);
        command.Parameters.AddWithValue("transactionTypeWithdraw", "W");
        command.Parameters.AddWithValue("transactionTypeTransfer", "T");

        // Return transaction count
        return (int)command.ExecuteScalar();
    }

    public int ValidAccountNumber(int accountNumber)
    {
        // Count accounts
        using var connection = new SqlConnection(ModelManger.ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = $"select count(*) from [Account] where AccountNumber = @accountNumber";
        command.Parameters.AddWithValue("accountNumber", accountNumber);

        // Return account count
        return (int)command.ExecuteScalar();
    }
}