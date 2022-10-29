using Microsoft.EntityFrameworkCore;
using PaymentTelephoneServices.Domain.DbModels;
using PaymentTelephoneServices.Infrastructure.Persistanse.DbMaps;

namespace PaymentTelephoneServices.Infrastructure.Persistanse;

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
