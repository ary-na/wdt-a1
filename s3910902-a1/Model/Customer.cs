namespace s3910902_a1.Model;

public class Customer
{
    public Customer()
    {
        
    }
    
    public int CustomerId { get; }
    public string Name { get; }
    public string Address { get; }
    public string City { get; }
    public int PostCode { get; set; }
}