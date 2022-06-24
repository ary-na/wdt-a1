using System.Data;
using Microsoft.Data.SqlClient;
using s3910902_a1.Dto;
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

        var withdraw = command.GetDataTable().Select()
            .Where(x => x.Field<char>("TransactionType") == 'W')
            .Select(x => new Withdraw
            {
                TransactionId = x.Field<int>("TransactionID"),
                TransactionType = x.Field<char>("TransactionType"),
                Amount = x.Field<decimal>("Amount"),
                Comment = x.Field<string?>("Comment"),
                TransactionTimeUtc = x.Field<DateTime>("TransactionTimeUtc"),
            }).ToList();

        var deposit = command.GetDataTable().Select()
            .Where(x => x.Field<char>("TransactionType") == 'D')
            .Select(x => new Deposit
            {
                TransactionId = x.Field<int>("TransactionID"),
                TransactionType = x.Field<char>("TransactionType"),
                Amount = x.Field<decimal>("Amount"),
                Comment = x.Field<string?>("Comment"),
                TransactionTimeUtc = x.Field<DateTime>("TransactionTimeUtc"),
            }).ToList();

        var transfer = command.GetDataTable().Select()
            .Where(x => x.Field<char>("TransactionType") == 'T')
            .Select(x => new Transfer
            {
                TransactionId = x.Field<int>("TransactionID"),
                TransactionType = x.Field<char>("TransactionType"),
                DestinationAccountNumber = x.Field<int>("DestinationAccountNumber"),
                Amount = x.Field<decimal>("Amount"),
                Comment = x.Field<string?>("Comment"),
                TransactionTimeUtc = x.Field<DateTime>("TransactionTimeUtc"),
            }).ToList();

        var service = command.GetDataTable().Select()
            .Where(x => x.Field<char>("TransactionType") == 'S')
            .Select(x => new Service()
            {
                TransactionId = x.Field<int>("TransactionID"),
                TransactionType = x.Field<char>("TransactionType"),
                Amount = x.Field<decimal>("Amount"),
                Comment = x.Field<string?>("Comment"),
                TransactionTimeUtc = x.Field<DateTime>("TransactionTimeUtc"),
            }).ToList();

        var transactions = new List<ITransaction>(withdraw.Count
                                                  + deposit.Count
                                                  + transfer.Count
                                                  + service.Count);
        transactions.AddRange(withdraw);
        transactions.AddRange(deposit);
        transactions.AddRange(transfer);
        transactions.AddRange(service);

        return transactions;
    }

    public void InsertTransaction(TransactionDto transactionDto)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        // Code sourced and adapted from:
        // https://social.msdn.microsoft.com/Forums/en-US/9f65826b-7d4d-4877-9630-3008bbb80157/need-help-systemdatasqlclientsqlexception-incorrect-syntax-near-the-keyword-read?forum=adodotnetdataproviders
        command.CommandText =
            @"insert into [Transaction] (TransactionType, AccountNumber, Amount, Comment, TransactionTimeUtc)
            values (@transactionType, @accountNumber, @amount, @comment, @transactionTimeUtc)";

        command.Parameters.AddWithValue("transactionType", transactionDto.TransactionType);
        command.Parameters.AddWithValue("accountNumber", transactionDto.AccountNumber);
        command.Parameters.AddWithValue("amount", transactionDto.Amount);
        command.Parameters.AddWithValue("comment", transactionDto.Comment?.GetObjectOrDbNull());
        command.Parameters.AddWithValue("transactionTimeUtc", transactionDto.TransactionTimeUtc);

        command.ExecuteNonQuery();
    }
}