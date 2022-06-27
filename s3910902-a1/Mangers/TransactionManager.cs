using System.Data;
using Microsoft.Data.SqlClient;
using s3910902_a1.DTOs;
using s3910902_a1.Models;
using s3910902_a1.Utilities;

namespace s3910902_a1.Mangers;

// Code sourced and adapted from:
// Week 3 Tutorial - PersonManager.cs
// https://rmit.instructure.com/courses/102750/files/24402824?wrap=1

public class TransactionManager
{
    private readonly string _connectionString;

    public TransactionManager(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<ITransaction> GetTransactions(int accountNumber)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = connection.CreateCommand();
        command.CommandText = "select * from [Transaction] where AccountNumber = @accountNumber";
        command.Parameters.AddWithValue("accountNumber", accountNumber);

        return command.GetDataTable().Select().Select(CreateTransactions).ToList();
    }

    public void InsertTransaction(TransactionDto transactionDto)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        
        // Code sourced and adapted from:
        // https://social.msdn.microsoft.com/Forums/en-US/9f65826b-7d4d-4877-9630-3008bbb80157/need-help-systemdatasqlclientsqlexception-incorrect-syntax-near-the-keyword-read?forum=adodotnetdataproviders
        // https://stackoverflow.com/questions/4488054/merge-two-or-more-lists-into-one-in-c-sharp-net

        command.CommandText =
            @"insert into [Transaction] (TransactionType, AccountNumber, Amount, Comment, TransactionTimeUtc)
            values (@transactionType, @accountNumber, @amount, @comment, @transactionTimeUtc)";

        command.Parameters.AddWithValue("transactionType", transactionDto.TransactionType);
        command.Parameters.AddWithValue("accountNumber", transactionDto.AccountNumber);
        command.Parameters.AddWithValue("amount", transactionDto.Amount);
        command.Parameters.AddWithValue("comment", transactionDto.Comment.GetObjectOrDbNull());
        command.Parameters.AddWithValue("transactionTimeUtc", transactionDto.TransactionTimeUtc);

        command.ExecuteNonQuery();
    }

    // Code sourced and adapted from:
    // Week 3 Lectorial - SwitchExpressions.cs
    // Week 3 Lectorial - Factory.cs
    // https://rmit.instructure.com/courses/102750/files/24463725?wrap=1
    
    private static ITransaction CreateTransactions(DataRow dataRow)
    {
        return dataRow.Field<string>("TransactionType") switch
        {
            "W" => new Withdraw
            {
                TransactionId = dataRow.Field<int>("TransactionID"),
                TransactionType = TransactionType.W,
                Amount = dataRow.Field<decimal>("Amount"),
                Comment = dataRow.Field<string?>("Comment"),
                TransactionTimeUtc = dataRow.Field<DateTime>("TransactionTimeUtc"),
            },
            "D" => new Deposit
            {
                TransactionId = dataRow.Field<int>("TransactionID"),
                TransactionType = TransactionType.D,
                Amount = dataRow.Field<decimal>("Amount"),
                Comment = dataRow.Field<string?>("Comment"),
                TransactionTimeUtc = dataRow.Field<DateTime>("TransactionTimeUtc"),
            },
            "T" => new Transfer
            {
                TransactionId = dataRow.Field<int>("TransactionID"),
                TransactionType = TransactionType.T,
                Amount = dataRow.Field<decimal>("Amount"),
                Comment = dataRow.Field<string?>("Comment"),
                TransactionTimeUtc = dataRow.Field<DateTime>("TransactionTimeUtc"),
            },
            "S" => new Service
            {
                TransactionId = dataRow.Field<int>("TransactionID"),
                TransactionType = TransactionType.S,
                Amount = dataRow.Field<decimal>("Amount"),
                Comment = dataRow.Field<string?>("Comment"),
                TransactionTimeUtc = dataRow.Field<DateTime>("TransactionTimeUtc"),
            },
            _ => throw new NullReferenceException()
        };
    }
}