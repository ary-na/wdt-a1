using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Data.SqlClient;
using s3910902_a1.Dto;

namespace s3910902_a1.Manger;

public class TransactionDtoManager
{
    private readonly string _connectionString;

    public TransactionDtoManager(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public void InsertTransaction(TransactionDto transactionDto)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText =
            @"insert into Transaction (TransactionID, TransactionType, AccountNumber, Amount, Comment, TransactionTimeUtc)
            values (@transactionId, @transactionType, @accountNumber, @amount, @comment, @transactionTimeUtc)";
        
        command.Parameters.AddWithValue("transactionId", transactionDto.TransactionId);
        command.Parameters.AddWithValue("transactionType", transactionDto.TransactionType);
        command.Parameters.AddWithValue("accountNumber", transactionDto.AccountNumber);
        command.Parameters.AddWithValue("amount", transactionDto.Amount);
        command.Parameters.AddWithValue("comment", transactionDto.Comment);
        command.Parameters.AddWithValue("transactionTimeUtc", transactionDto.TransactionTimeUtc);

        command.ExecuteNonQuery();
    }
}