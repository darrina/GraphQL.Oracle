using HotChocolate;
using GraphQLOracleApi.Data.Entities;

namespace GraphQLOracleApi.Data;

public class MutationType
{
    private readonly OracleDbContext _context;

    public MutationType(OracleDbContext context)
    {
        _context = context;
    }

    public async Task<Customer> CreateCustomer(Customer input)
    {
        _context.Customers.Add(input);
        await _context.SaveChangesAsync();
        return input;
    }
}