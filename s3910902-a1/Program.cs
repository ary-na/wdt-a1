using s3910902_a1.Mangers;
using s3910902_a1.Menus;
using s3910902_a1.Services;

namespace s3910902_a1;

// Code sourced and adapted from:
// Week 3 Tutorial - Program.cs
// https://rmit.instructure.com/courses/102750/files/24402824?wrap=1

// Most Common Bank of Australia console application
public static class Program
{
    private static async Task Main()
    {
        var model = new ModelManger();
        await Task.WhenAny(CustomerWebService.GetAndSaveCustomer(model.ConnectionString));
        LoginMenu.Run(model);
    }
}