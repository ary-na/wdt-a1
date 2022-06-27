using Microsoft.Extensions.Configuration;
using s3910902_a1.Models;
using s3910902_a1.Services;

namespace s3910902_a1.Mangers;

public class ModelManger
{
    private static readonly IConfiguration Configuration =
        new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

    public string ConnectionString { get; }

    public ModelManger()
    {
        ConnectionString = Configuration.GetConnectionString(nameof(CustomerWebService)) ??
                           throw new NullReferenceException();
    }
}