using PaymentTelephoneServices.Application.Contracts;
using PaymentTelephoneServices.Domain.Models;

namespace PaymentTelephoneServices.Infrastructure.Services;

internal class PaymentTransactionsDbService : IPaymentTransactionsDbService
{
    public Task SavePaymentTransactionAsync(Payment payment, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
