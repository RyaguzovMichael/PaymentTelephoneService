using Microsoft.Extensions.Logging;
using PaymentTelephoneServices.Domain.Models;
using PaymentTelephoneServices.Infrastructure.Services.Interfaces;
using System.Text.Json;

namespace PaymentTelephoneServices.Infrastructure.Services.MobileOperatorServices
{
    internal class ActiveMobileOperatorService : IMobileOperatorService
    {
        private readonly ILogger<ActiveMobileOperatorService> _logger;
        private readonly JsonSerializerOptions _serializerOptions;

        public ActiveMobileOperatorService(ILogger<ActiveMobileOperatorService> logger)
        {
            _logger = logger;
            _serializerOptions = new()
            {
                WriteIndented = true
            };
        }

        public Task<bool> SendPaymentAsync(Payment payment, CancellationToken cancellationToken)
        {
            _logger.LogInformation(JsonSerializer.Serialize(payment, _serializerOptions) + " - payment is send to Active successful");
            return Task.FromResult(true);
        }
    }
}
