using PaymentTelephoneServices.Application.Contracts;
using PaymentTelephoneServices.Domain.Models;

namespace PaymentTelephoneServices.API.Tests.Repository;

internal class MoqMobileOperatorService : IMobileOperatorService
{
    public Task<bool> SendPaymentAsync(Payment payment, CancellationToken cancellationToken)
    {
        return Task.FromResult(true);
    }
}
