using PaymentTelephoneServices.Domain.Models;

namespace PaymentTelephoneServices.Application.Contracts;

internal interface IMobileOperatorServicesAggregator
{
    Task<bool> SendPaymentAsync(Payment payment, CancellationToken cancellationToken);
}
