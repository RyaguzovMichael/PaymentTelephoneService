using Microsoft.EntityFrameworkCore;
using PaymentTelephoneServices.Domain.DbModels;
using PaymentTelephoneServices.Infrastructure.Persistence.DbMaps;

namespace PaymentTelephoneServices.Infrastructure.Persistence;

internal class TransactionsContext : DbContext
{
    public DbSet<Operator> Operators { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    public TransactionsContext(DbContextOptions<TransactionsContext> contextOptions) : base(contextOptions)
    {
        Database.Migrate();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new TransactionDbMap());
    }
}
