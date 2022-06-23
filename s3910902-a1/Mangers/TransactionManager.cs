using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Data.SqlClient;
using s3910902_a1.Dto;

namespace s3910902_a1.Manger;

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
        
        command.Parameters.AddWithValue("transactionType", GetObjectOrDbNull(transactionDto.TransactionType));
        command.Parameters.AddWithValue("accountNumber", GetObjectOrDbNull(transactionDto.AccountNumber));
        command.Parameters.AddWithValue("amount", GetObjectOrDbNull(transactionDto.Amount));
        command.Parameters.AddWithValue("comment", GetObjectOrDbNull(transactionDto.Comment));
        command.Parameters.AddWithValue("transactionTimeUtc", transactionDto.TransactionTimeUtc);

        command.ExecuteNonQuery();
    }
    private object GetObjectOrDbNull(object value) => value ?? DBNull.Value;
}