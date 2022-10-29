using Microsoft.Extensions.Options;
using PaymentTelephoneServices.Domain.OptionModels;
using System.Text.Json;

namespace PaymentTelephoneServices.API.DependencyInjection;

internal static class DependencyInjection
{
    public static IServiceCollection AddApiOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OperatorCodesOptions>(configuration.GetSection("OperatorCodesOptions"));

        return services;
    }
}
