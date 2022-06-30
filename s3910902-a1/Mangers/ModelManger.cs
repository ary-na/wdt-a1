using Microsoft.Extensions.Configuration;
using s3910902_a1.Services;

namespace s3910902_a1.Mangers;

// Code sourced and adapted from:
// Week 3 Lectorial - LoggerFactory.cs
// https://rmit.instructure.com/courses/102750/files/24463725?wrap=1

public class ModelManger
{
    private static readonly IConfiguration Configuration =
        new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

    public string ConnectionString { get; }

    public ModelManger()
    {
        ConnectionString = Configuration.GetConnectionString(nameof(CustomerWebService)) ?? throw new NullReferenceException();
    }
}