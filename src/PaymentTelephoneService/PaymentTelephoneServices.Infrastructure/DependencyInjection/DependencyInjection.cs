using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentTelephoneServices.Application.Contracts;
using PaymentTelephoneServices.Application.DependencyInjection;
using PaymentTelephoneServices.Domain.Exceptions;
using PaymentTelephoneServices.Infrastructure.Persistence;
using PaymentTelephoneServices.Infrastructure.Services;
using PaymentTelephoneServices.Infrastructure.Services.MobileOperatorServices;

namespace PaymentTelephoneServices.Infrastructure.DependencyInjection;

internal static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<ActiveMobileOperatorService>();
        services.AddTransient<AltelMobileOperatorService>();
        services.AddTransient<BeelineMobileOperatorService>();
        services.AddTransient<TeleTwoMobileOperatorService>();

        services.AddTransient<MobileOperatorServiceResolver>(provider => operatorName =>
        {
            return operatorName switch
            {
                "Active" => provider.GetRequiredService<ActiveMobileOperatorService>(),
                "Altel" => provider.GetRequiredService<AltelMobileOperatorService>(),
                "Beeline" => provider.GetRequiredService<BeelineMobileOperatorService>(),
                "Tele2" => provider.GetRequiredService<TeleTwoMobileOperatorService>(),
                _ => throw new MobileOperatorServiceIsNotPresented(
                                $"Mobile operator service is not presented by name: {operatorName}"),
            };
        });
        
        services.AddDbContext<TransactionsContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DbConnectionString")));
        services.AddTransient<IPaymentTransactionsDbService, PaymentTransactionsDbService>();

        return services;
    }
}
