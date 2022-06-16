using Microsoft.Extensions.Configuration;
using s3910902_a1.Service;

var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
var connectionString = configuration.GetConnectionString(nameof(CustomerWebService));
CustomerWebService.GetAndSaveCustomers();

// Most Common Bank of Australia console application
//MostCommonBankOfAustraliaMenu.Run();