using PaymentTelephoneServices.Domain.Models;

namespace PaymentTelephoneServices.Application.Contracts;

internal interface IMobileOperatorService
{
    Task<bool> SendPaymentAsync(Payment payment, CancellationToken cancellationToken);
}
