using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using s3910902_a1.Mangers;
using Utilities.DTOs;

namespace s3910902_a1.Services;

// Code sourced and adapted from:
// Week 3 Tutorial - PersonWebService.cs
// Week 3 Tutorial - Primes.cs
// https://rmit.instructure.com/courses/102750/files/24402824?wrap=1

public static class CustomerWebService
{
    public static async Task GetAndSaveCustomer(string connectionString)
    {
        var loginManager = new LoginManager(connectionString);

        // Login details exist in database condition
        if (loginManager.Logins.Any()) return;

        const string url = "https://coreteaching01.csit.rmit.edu.au/~e103884/wdt/services/customers/";
        using var client = new HttpClient();
        var json = client.GetStringAsync(url).Result;

        // Deserialize
        var customerDto = JsonConvert.DeserializeObject<List<CustomerDto>>(json, new JsonSerializerSettings
        {
            // Code sourced and adapted from:
            // https://docs.microsoft.com/en-au/dotnet/standard/base-types/custom-date-and-time-format-strings
            DateFormatString = "dd/MM/yyyy hh:mm:ss tt"
        });

        // Insert objects into database
        await Task.WhenAny(InsertCustomerDto(connectionString, customerDto));
    }

    // Code sourced and adapted from:
    // https://stackoverflow.com/questions/29294582/what-are-the-benefits-of-inserting-the-data-into-the-database-asynchronously
    // https://www.c-sharpcorner.com/article/async-and-await-in-c-sharp/
    // https://stackoverflow.com/questions/68859259/how-to-await-until-parallel-task-done
    // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/start-multiple-async-tasks-and-process-them-as-they-complete?pivots=dotnet-6-0
    // https://docs.microsoft.com/en-us/archive/msdn-magazine/2013/march/async-await-best-practices-in-asynchronous-programming

    private static async Task InsertCustomerDto(string connectionString, List<CustomerDto>? customerDto)
    {
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        // Insert objects into database
        foreach (var customer in customerDto)
        {
            await Task.WhenAny(CustomerManager.InsertCustomer(connection, customer));
            customer.Login.CustomerId = customer.CustomerId;
            await Task.WhenAny(LoginManager.InsertLogin(connection, customer.Login));

            foreach (var account in customer.Accounts)
            {
                foreach (var accountTransaction in account.Transactions)
                {
                    account.Balance += accountTransaction.Amount;
                }

                await Task.WhenAny(AccountManager.InsertAccount(connection, account));

                foreach (var accountTransaction in account.Transactions)
                {
                    accountTransaction.AccountNumber = account.AccountNumber;
                    await Task.WhenAny(TransactionManager.InsertTransaction(connection, accountTransaction));
                }
            }
        }
    }
}