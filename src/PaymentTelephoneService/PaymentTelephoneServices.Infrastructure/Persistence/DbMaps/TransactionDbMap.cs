using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentTelephoneServices.Domain.DbModels;

namespace PaymentTelephoneServices.Infrastructure.Persistence.DbMaps;

internal class TransactionDbMap : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.Property(p => p.TransactionTime).HasColumnType("TIMESTAMP");
    }
}
