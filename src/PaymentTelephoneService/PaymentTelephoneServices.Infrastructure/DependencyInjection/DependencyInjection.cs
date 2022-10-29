using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PaymentTelephoneServices.Application.Contracts;
using PaymentTelephoneServices.Infrastructure.DependencyInjection.Options;
using PaymentTelephoneServices.Infrastructure.Persistanse;
using PaymentTelephoneServices.Infrastructure.Services;
using PaymentTelephoneServices.Infrastructure.Services.MobileOperatorServices;

namespace PaymentTelephoneServices.Infrastructure.DependencyInjection;

internal static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var logger = services.BuildServiceProvider().GetRequiredService<ILogger<MobileOperatorServiceAggregatorOptions>>();
        services.AddSingleton(provider => new MobileOperatorServiceAggregatorOptions(configuration => 
        {
            configuration.Add("701", typeof(ActiveMobileOperatorService));
            configuration.Add("777", typeof(BeelineMobileOperatorService));
            configuration.Add("705", typeof(BeelineMobileOperatorService));
            configuration.Add("707", typeof(TeleTwoMobileOperatorService));
            configuration.Add("747", typeof(TeleTwoMobileOperatorService));
            configuration.Add("700", typeof(AltelMobileOperatorService));
            configuration.Add("708", typeof(AltelMobileOperatorService));
        }, logger));

        services.AddTransient<IMobileOperatorServicesAggregator, MobileOperatorServicesAggregator>();
        services.AddDbContext<TransactionsContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DbConnectionString")));

        services.AddTransient<IPaymentTransactionsDbService, PaymentTransactionsDbService>();

        return services;
    }
}
