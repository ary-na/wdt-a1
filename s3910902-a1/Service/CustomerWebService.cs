using System.Transactions;
using Newtonsoft.Json;
using s3910902_a1.Dto;
using s3910902_a1.Manger;

namespace s3910902_a1.Service;

// Code sourced and adapted from:
// Week 3 Tutorial - PersonWebService.cs
// https://rmit.instructure.com/courses/102750/files/24402824?wrap=1
public static class CustomerWebService
{
    public static void GetAndSaveCustomer(string connectionString)
    {
        const string url = "https://coreteaching01.csit.rmit.edu.au/~e103884/wdt/services/customers/";
        using var client = new HttpClient();
        var json = client.GetStringAsync(url).Result;

        var customerDto = JsonConvert.DeserializeObject<List<CustomerDto>>(json, new JsonSerializerSettings
        {
            // Code sourced and adapted from:
            // https://docs.microsoft.com/en-au/dotnet/standard/base-types/custom-date-and-time-format-strings
            DateFormatString = "dd/MM/yyyy hh:mm:ss tt"
        });

        var customerDtoManager = new CustomerDtoManager(connectionString);
        var loginDtoManager = new LoginDtoManager(connectionString);
        var accountDtoManager = new AccountDtoManager(connectionString);
        var transactionDtoManager = new TransactionDtoManager(connectionString);

        foreach (var customer in customerDto)
        {
            customerDtoManager.InsertCustomer(customer);
            customer.Login.CustomerId = customer.CustomerId;
            loginDtoManager.InsertLogin(customer.Login);

            foreach (var account in customer.Accounts)
            {
                foreach (var transaction in account.Transactions)
                {
                    account.Balance += transaction.Amount;
                }

                accountDtoManager.InsertAccount(account);

                foreach (var transaction in account.Transactions)
                {
                    transaction.AccountNumber = account.AccountNumber;
                    transactionDtoManager.InsertTransaction(transaction);
                }
            }
        }
    }
}