using GraphQLOracleApi.Data.Entities;

namespace GraphQLOracleApi.Data;

public class SeedData
{
    private readonly OracleDbContext _dbContext;

    public SeedData(OracleDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void SeedDatabase()
    {
        if (_dbContext.Customers.Any())
        {
            return; // Database is already seeded
        }

        // Add some sample data
        var address1 = new Address { StreetAddress = "123 Main St", City = "Anytown", State = "NY", PostalCode = "12345", Country = "USA"  };
        var address2 = new Address { StreetAddress = "456 Elm St", City = "Othertown", State = "CA", PostalCode = "54321", Country = "USA"  };
        _dbContext.Addresses.AddRange(address1, address2);
        _dbContext.SaveChanges();

        var customer1 = new Customer { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", PrimaryAddress = address1 };
        var customer2 = new Customer { FirstName = "Jane", LastName = "Doe", Email = "jane.doe@example.com", PrimaryAddress = address2 };
        _dbContext.Customers.AddRange(customer1, customer2);
        _dbContext.SaveChanges();

        var order1 = new Order { CustomerId = customer1.Id, OrderDate = DateTime.Now, TotalAmount = 100.00M };
        var order2 = new Order { CustomerId = customer2.Id, OrderDate = DateTime.Now, TotalAmount = 50.00M };
        _dbContext.Orders.AddRange(order1, order2);
        _dbContext.SaveChanges();

        var payment1 = new Payment { OrderId = order1.Id, PaymentMethod = PaymentMethod.CreditCard, PaymentDate = DateTime.Now, Amount = 50.00M };
        var payment2 = new Payment { OrderId = order1.Id, PaymentMethod = PaymentMethod.DebitCard, PaymentDate = DateTime.Now, Amount = 50.00M };
        var payment3 = new Payment { OrderId = order2.Id, PaymentMethod = PaymentMethod.PayPal, PaymentDate = DateTime.Now, Amount = 50.00M };
        _dbContext.Payments.AddRange(payment1, payment2, payment3);
        _dbContext.SaveChanges();
    }
}