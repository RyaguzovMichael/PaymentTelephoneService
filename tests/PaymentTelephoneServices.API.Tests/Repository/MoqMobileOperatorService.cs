using PaymentTelephoneServices.Domain.Models;
using PaymentTelephoneServices.Infrastructure.Services.Interfaces;

namespace PaymentTelephoneServices.API.Tests.Repository;

internal class MoqMobileOperatorService : IMobileOperatorService
{
    public Task<bool> SendPaymentAsync(Payment payment, CancellationToken cancellationToken)
    {
        return Task.FromResult(true);
    }
}
