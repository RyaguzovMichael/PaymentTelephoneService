using PaymentTelephoneServices.Application.Contracts;
using PaymentTelephoneServices.Domain.Models;

namespace PaymentTelephoneServices.API.Tests.Repository;

internal class MoqRepository : IPaymentTransactionsDbService
{
    public Task SavePaymentTransactionAsync(Payment payment, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task SetMobileOperatorsData(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
