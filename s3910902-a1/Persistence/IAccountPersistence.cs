using s3910902_a1.Models;

namespace s3910902_a1.Persistence;

// Code sourced and adapted from:
// Week 3 Lectorial - IAnimalPersistence.cs
// https://rmit.instructure.com/courses/102750/files/24463725?wrap=1

public interface IAccountPersistence
{
    public ITransaction InsertTransaction(ITransaction transaction);
    public decimal UpdateBalance(int accountNumber, decimal balance);
}