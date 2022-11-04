using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PaymentTelephoneServices.Application.Contracts;
using PaymentTelephoneServices.Application.DependencyInjection;
using PaymentTelephoneServices.Domain.Exceptions;
using PaymentTelephoneServices.Domain.OptionModels;

namespace PaymentTelephoneServices.Application.Features.SetPayment;

internal class SetPaymentCommandHandler : IRequestHandler<SetPaymentCommand, bool>
{
    private readonly ILogger<SetPaymentCommandHandler> _logger;
    private readonly IPaymentTransactionsDbService _paymentTransactionsDbService;
    private readonly MobileOperatorServiceResolver _mobileOperatorServiceResolver;
    private readonly IOptions<OperatorCodesOptions> _options;

    public SetPaymentCommandHandler(MobileOperatorServiceResolver mobileOperatorServiceResolver,
                                    IPaymentTransactionsDbService paymentTransactionsDbService,
                                    ILogger<SetPaymentCommandHandler> logger,
                                    IOptions<OperatorCodesOptions> options)
    {
        _mobileOperatorServiceResolver = mobileOperatorServiceResolver;
        _paymentTransactionsDbService = paymentTransactionsDbService;
        _logger = logger;
        _options = options;
    }

    public async Task<bool> Handle(SetPaymentCommand request, CancellationToken cancellationToken)
    {
        string? operatorName = _options.Value.GetOperatorNameByCode(request.Payment.PhoneNumber.OperatorCode);
        if (operatorName is null) 
            throw new MobileOperatorServiceIsNotPresented($"Mobile operator is not presented by " +
                                                          $"operator code: {request.Payment.PhoneNumber.OperatorCode}");
        IMobileOperatorService mobileOperatorService = _mobileOperatorServiceResolver.Invoke(operatorName);
        if (!await mobileOperatorService.SendPaymentAsync(request.Payment, cancellationToken)) return false;
        await _paymentTransactionsDbService.SavePaymentTransactionAsync(request.Payment, cancellationToken);
        _logger.LogInformation($"Payment to number: {request.Payment.PhoneNumber} is successfully complete.");
        return true;
    }
}
