using Newtonsoft.Json;
using s3910902_a1.Dto;

namespace s3910902_a1.Service;

// Code sourced and adapted from:
// Week 3 Tutorial - PersonWebService.cs
// https://rmit.instructure.com/courses/102750/files/24402824?wrap=1
public static class CustomerWebService
{
    private static List<CustomerDto>? CustomerDto { get; set; }

    public static void GetAndSaveCustomers()
    {
        const string url = "https://coreteaching01.csit.rmit.edu.au/~e103884/wdt/services/customers/";
        using var client = new HttpClient();
        var json = client.GetStringAsync(url).Result;

        CustomerDto = JsonConvert.DeserializeObject<List<CustomerDto>>(json, new JsonSerializerSettings
        {
            // Code sourced and adapted from:
            // https://docs.microsoft.com/en-au/dotnet/standard/base-types/custom-date-and-time-format-strings
            DateFormatString = "dd/MM/yyyy hh:mm:ss tt"
        });
    }
}