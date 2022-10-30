using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PaymentTelephoneServices.Application.Contracts;
using PaymentTelephoneServices.Domain.Exceptions;
using PaymentTelephoneServices.Domain.Models;
using PaymentTelephoneServices.Infrastructure.DependencyInjection.Options;
using PaymentTelephoneServices.Infrastructure.Services.Interfaces;

namespace PaymentTelephoneServices.Infrastructure.Services;

internal class MobileOperatorServicesAggregator : IMobileOperatorServicesAggregator
{
    private readonly MobileOperatorServiceAggregatorOptions _options;
    private readonly ILogger<MobileOperatorServicesAggregator> _logger;
    private readonly IServiceProvider _serviceProvider;

    public MobileOperatorServicesAggregator(ILogger<MobileOperatorServicesAggregator> logger,
                                            MobileOperatorServiceAggregatorOptions options,
                                            IServiceProvider provider)
    {
        _logger = logger;
        _options = options;
        _serviceProvider = provider;
    }

    public async Task<bool> SendPaymentAsync(Payment payment, CancellationToken cancellationToken)
    {
        if (!_options.CodeOperatorServicePairs.TryGetValue(payment.PhoneNumber.OperatorCode, out Type? serviceType))
            throw new MobileOperatorServiceIsNotPresented(
                $"Mobile operator service is not presented for mobile code: {payment.PhoneNumber.OperatorCode}");
        try
        {
            var service = (IMobileOperatorService)ActivatorUtilities.CreateInstance(_serviceProvider, serviceType!);
            return await service.SendPaymentAsync(payment, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }
}
