using Microsoft.Extensions.Logging;
using PaymentTelephoneServices.Application.Contracts;
using PaymentTelephoneServices.Domain.Models;
using System.Text.Json;

namespace PaymentTelephoneServices.Infrastructure.Services.MobileOperatorServices;

internal class TeleTwoMobileOperatorService : IMobileOperatorService
{
    private readonly ILogger<TeleTwoMobileOperatorService> _logger;
    private readonly JsonSerializerOptions _serializerOptions;

    public TeleTwoMobileOperatorService(ILogger<TeleTwoMobileOperatorService> logger)
    {
        _logger = logger;
        _serializerOptions = new JsonSerializerOptions()
        {
            WriteIndented = true
        };
    }

    public Task<bool> SendPaymentAsync(Payment payment, CancellationToken cancellationToken)
    {
        _logger.LogInformation(JsonSerializer.Serialize(payment, _serializerOptions) + " - payment is send to Tele-2 successful");
        return Task.FromResult(true);
    }
}
