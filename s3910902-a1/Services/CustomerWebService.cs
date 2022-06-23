using Newtonsoft.Json;
using s3910902_a1.Dto;
using s3910902_a1.Manger;

namespace s3910902_a1.Services;

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

        var customerManager = new CustomerManager(connectionString);
        var loginManager = new LoginManager(connectionString);
        var accountManager = new AccountManager(connectionString);
        var transactionManager = new TransactionManager(connectionString);
        
        InsertCustomerDto(customerDto, customerManager, loginManager, accountManager, transactionManager);
    }

    private static void InsertCustomerDto(List<CustomerDto>? customerDto, CustomerManager customerManager,
        LoginManager loginManager, AccountManager accountManager, TransactionManager transactionManager)
    {
        foreach (var customer in customerDto)
        {
            customerManager.InsertCustomer(customer);
            customer.Login.CustomerId = customer.CustomerId;
            loginManager.InsertLogin(customer.Login);

            foreach (var account in customer.Accounts)
            {
                foreach (var transaction in account.Transactions)
                {
                    account.Balance += transaction.Amount;
                }

                accountManager.InsertAccount(account);

                foreach (var transaction in account.Transactions)
                {
                    transaction.AccountNumber = account.AccountNumber;
                    transactionManager.InsertTransaction(transaction);
                }
            }
        }
    }
}