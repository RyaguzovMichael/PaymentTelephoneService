using MediatR;
using Microsoft.Extensions.Logging;
using PaymentTelephoneServices.Application.Contracts;

namespace PaymentTelephoneServices.Application.Features.SetPayment;

internal class SetPaymentCommandHandler : IRequestHandler<SetPaymentCommand, bool>
{
    private readonly ILogger<SetPaymentCommandHandler> _logger;
    private readonly IMobileOperatorServicesAggregator _mobileOperatorServicesAggregator;
    private readonly IPaymentTransactionsDbService _paymentTransactionsDbService;

    public SetPaymentCommandHandler(IMobileOperatorServicesAggregator mobileOperatorServicesAggregator,
                                    IPaymentTransactionsDbService paymentTransactionsDbService,
                                    ILogger<SetPaymentCommandHandler> logger)
    {
        _mobileOperatorServicesAggregator = mobileOperatorServicesAggregator;
        _paymentTransactionsDbService = paymentTransactionsDbService;
        _logger = logger;
    }

    public async Task<bool> Handle(SetPaymentCommand request, CancellationToken cancellationToken)
    {
        if(await _mobileOperatorServicesAggregator.SendPaymentAsync(request.Payment, cancellationToken))
        {
            await _paymentTransactionsDbService.SavePaymentTransactionAsync(request.Payment, cancellationToken);
            _logger.LogInformation($"Payment to number: {request.Payment.PhoneNumber} is sucsessfuly complete.");
            return true;
        }
        return false;
    }
}
