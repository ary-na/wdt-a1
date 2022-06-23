using Microsoft.Data.SqlClient;
using s3910902_a1.Dto;

namespace s3910902_a1.Manger;

// Code sourced and adapted from:
// Week 3 Tutorial - PersonManager.cs
// https://rmit.instructure.com/courses/102750/files/24402824?wrap=1

public class CustomerManager
{
    private readonly string _connectionString;

    public CustomerManager(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void InsertCustomer(CustomerDto customerDto)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText =
            @"insert into [Customer] (CustomerID, Name, Address, City, PostCode)
            values (@customerId, @name, @address, @city, @postCode)";
        
        command.Parameters.AddWithValue("customerId", GetObjectOrDbNull(customerDto.CustomerId));
        command.Parameters.AddWithValue("name", GetObjectOrDbNull(customerDto.Name));
        command.Parameters.AddWithValue("address", GetObjectOrDbNull(customerDto.Address));
        command.Parameters.AddWithValue("city", GetObjectOrDbNull(customerDto.City));
        command.Parameters.AddWithValue("postCode", GetObjectOrDbNull(customerDto.PostCode));

        command.ExecuteNonQuery();
    }
    
    public object GetObjectOrDbNull(object value) => value ?? DBNull.Value;
}