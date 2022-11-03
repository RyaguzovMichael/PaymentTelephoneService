using Microsoft.Extensions.Logging;
using PaymentTelephoneServices.Application.Contracts;
using PaymentTelephoneServices.Domain.Models;
using System.Text.Json;

namespace PaymentTelephoneServices.Infrastructure.Services.MobileOperatorServices;

internal class AltelMobileOperatorService : IMobileOperatorService
{
    private readonly ILogger<AltelMobileOperatorService> _logger;
    private readonly JsonSerializerOptions _serializerOptions;

    public AltelMobileOperatorService(ILogger<AltelMobileOperatorService> logger)
    {
        _logger = logger;
        _serializerOptions = new()
        {
            WriteIndented = true
        };
    }

    public Task<bool> SendPaymentAsync(Payment payment, CancellationToken cancellationToken)
    {
        _logger.LogInformation(JsonSerializer.Serialize(payment, _serializerOptions) + " - payment is send to Altel sucsessful");
        return Task.FromResult(true);
    }
}
