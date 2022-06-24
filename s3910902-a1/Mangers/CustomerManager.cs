using System.Data;
using Microsoft.Data.SqlClient;
using s3910902_a1.Dto;
using s3910902_a1.Models;
using s3910902_a1.Utilities;

namespace s3910902_a1.Mangers;

// Code sourced and adapted from:
// Week 3 Tutorial - PersonManager.cs
// https://rmit.instructure.com/courses/102750/files/24402824?wrap=1

public class CustomerManager
{
    private readonly string _connectionString;

    private readonly LoginManager _loginManager;
    public Customer Customer { get; }

    public CustomerManager(string connectionString)
    {
        _connectionString = connectionString;
        _loginManager = new LoginManager(connectionString);
        // Code sourced and adapted from:
        // https://www.jetbrains.com/help/resharper/InvertIf.html
        //var loginManger = new LoginManager(_connectionString);
        //if (!loginManger.ValidLogin) return;

        using var connection = new SqlConnection(_connectionString);
        using var command = connection.CreateCommand();
        command.CommandText = "select * from [Customer] where CustomerID = @customerId";
        command.Parameters.AddWithValue("customerId", _loginManager.Login.CustomerId);

        var accountManager = new AccountManager(_connectionString);

        Customer = command.GetDataTable().Select().Select(x => new Customer
        {
            CustomerId = x.Field<int>("CustomerID"),
            Name = x.Field<string?>("Name"),
            Address = x.Field<string?>("Address"),
            City = x.Field<string?>("City"),
            PostCode = x.Field<string?>("PostCode"),
            //Accounts = accountManager.GetAccounts(x.Field<int>("CustomerID")),
            //Login = _loginManager.Login
        }).First();
    }

    public void InsertCustomer(CustomerDto customerDto)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText =
            @"insert into [Customer] (CustomerID, Name, Address, City, PostCode)
            values (@customerId, @name, @address, @city, @postCode)";

        // Code sourced and adapted from:
        // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/how-to-implement-and-call-a-custom-extension-method 
        command.Parameters.AddWithValue("customerId", customerDto.CustomerId);
        command.Parameters.AddWithValue("name", customerDto.Name);
        command.Parameters.AddWithValue("address", customerDto.Address?.GetObjectOrDbNull());
        command.Parameters.AddWithValue("city", customerDto.City?.GetObjectOrDbNull());
        command.Parameters.AddWithValue("postCode", customerDto.PostCode?.GetObjectOrDbNull());

        command.ExecuteNonQuery();
    }
}