using PaymentTelephoneServices.Domain.Models;

namespace PaymentTelephoneServices.Infrastructure.Services.Interfaces;

internal interface IMobileOperatorService
{
    Task<bool> SendPaymentAsync(Payment payment, CancellationToken cancellationToken);
}
