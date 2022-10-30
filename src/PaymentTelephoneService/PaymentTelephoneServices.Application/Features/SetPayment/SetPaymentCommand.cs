using MediatR;
using PaymentTelephoneServices.Domain.Models;

namespace PaymentTelephoneServices.Application.Features.SetPayment;

internal class SetPaymentCommand : IRequest<bool>
{
    public Payment Payment { get; init; }
}
