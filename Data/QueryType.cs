using HotChocolate;
using GraphQLOracleApi.Data.Entities;

namespace GraphQLOracleApi.Data;
public class QueryType
{
    private readonly OracleDbContext _context;

    public QueryType(OracleDbContext context)
    {
        _context = context;
    }

    public IQueryable<Customer> GetCustomers()
    {
        return _context.Customers;
    }

    public IQueryable<Order> GetOrders()
    {
        return _context.Orders;
    }
}