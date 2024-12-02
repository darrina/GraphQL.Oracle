using Microsoft.EntityFrameworkCore;
// using Oracle.EntityFrameworkCore;
using GraphQLOracleApi.Data.Entities;

namespace GraphQLOracleApi.Data;

public class OracleDbContext : DbContext
{
    public OracleDbContext(DbContextOptions<OracleDbContext> options)
        : base(options)
    {
    }

    // DbSets
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Address> Addresses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Relationships and Constraints
        modelBuilder.Entity<Customer>()
            .HasMany(c => c.Orders)
            .WithOne(o => o.Customer)
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Customer>()
            .HasOne(c => c.PrimaryAddress)
            .WithOne(a => a.Customer)
            .HasForeignKey<Address>(a => a.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Order>()
            .HasMany(o => o.Payments)
            .WithOne(p => p.Order)
            .HasForeignKey(p => p.OrderId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}