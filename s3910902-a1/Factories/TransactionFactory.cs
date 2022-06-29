using s3910902_a1.Models;

namespace s3910902_a1.Factories;

// Code sourced and adapted from:
// Week 3 Lectorial - ShapeFactory.cs
// https://rmit.instructure.com/courses/102750/files/24463725?wrap=1

public static class TransactionFactory
{
    public static ITransaction Create(TransactionType transactionType)
    {
        return transactionType switch
        {
            TransactionType.Deposit => new Deposit
            {
                
            },
            TransactionType.Withdraw => new Withdraw
            {
                
            },
            TransactionType.Transfer => new Transfer
            {
                
            },
            TransactionType.Service => new Service(),
            _ => throw new NotSupportedException("Invalid transaction type.")
        };
    }
}