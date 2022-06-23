using Microsoft.Extensions.Configuration;
using s3910902_a1;
using s3910902_a1.Services;

var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
var connectionString = configuration.GetConnectionString(nameof(CustomerWebService));

//CustomerWebService.GetAndSaveCustomer(connectionString);

Login.Run(connectionString);

// Most Common Bank of Australia console application
//Menu.Run();