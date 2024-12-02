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
        if (!_dbContext.Model.FindEntityType(typeof(Customer))?.ClrType.IsAssignableFrom(typeof(Customer)) ?? false || _dbContext.Customers.Any())
        {
            return; // Database is already seeded
        }


        // Add some sample data
        var customer1 = new Customer { FirstName = "John", LastName = "Doe" };
        var customer2 = new Customer { FirstName = "Jane", LastName = "Doe" };

        _dbContext.Customers.AddRange(customer1, customer2);
        _dbContext.SaveChanges();

        var order1 = new Order { CustomerId = customer1.Id, OrderDate = DateTime.Now, TotalAmount = 100.00M };
        var order2 = new Order { CustomerId = customer2.Id, OrderDate = DateTime.Now, TotalAmount = 50.00M };

        _dbContext.Orders.AddRange(order1, order2);
        _dbContext.SaveChanges();

        var payment1 = new Payment { OrderId = order1.Id, PaymentDate = DateTime.Now, Amount = 50.00M };
        var payment2 = new Payment { OrderId = order1.Id, PaymentDate = DateTime.Now, Amount = 50.00M };
        var payment3 = new Payment { OrderId = order2.Id, PaymentDate = DateTime.Now, Amount = 50.00M };

        _dbContext.Payments.AddRange(payment1, payment2, payment3);
        _dbContext.SaveChanges();
    }
}