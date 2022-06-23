using Microsoft.Extensions.Configuration;
using s3910902_a1;
using s3910902_a1.Services;

// Code sourced and adapted from:
// Week 3 Tutorial - Program.cs
// https://rmit.instructure.com/courses/102750/files/24402824?wrap=1

var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
var connectionString = configuration.GetConnectionString(nameof(CustomerWebService));

//CustomerWebService.GetAndSaveCustomer(connectionString);

Login.Run(connectionString);

// Most Common Bank of Australia console application
//Menu.Run();