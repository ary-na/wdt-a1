using Microsoft.Data.SqlClient;
using s3910902_a1.Mangers;
using s3910902_a1.Models;
using s3910902_a1.Utilities;

namespace s3910902_a1.Persistence;

public class AccountPersistence : IAccountPersistence
{
    private static readonly ModelManger ModelManger = new();

    public ITransaction InsertTransaction(ITransaction transaction)
    {
        // Insert trnasaction
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
        // Update database
        using var connection = new SqlConnection(ModelManger.ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = $"update [Account] set Balance = @balance where AccountNumber = @accountNumber";
        command.Parameters.AddWithValue("balance", balance);
        command.Parameters.AddWithValue("accountNumber", accountNumber);

        command.ExecuteNonQuery();

        // Return Balance
        return balance;
    }
}