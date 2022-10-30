using PaymentTelephoneServices.Domain.Models;

namespace PaymentTelephoneServices.Application.Contracts;

internal interface IPaymentTransactionsDbService
{
    Task SavePaymentTransactionAsync(Payment payment, CancellationToken cancellationToken);
    Task SetMobileOperatorsData(CancellationToken cancellationToken);
}
