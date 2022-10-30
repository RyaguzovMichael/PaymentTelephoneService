using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PaymentTelephoneServices.Application.Contracts;
using PaymentTelephoneServices.Infrastructure.DependencyInjection.Options;
using PaymentTelephoneServices.Infrastructure.Persistence;
using PaymentTelephoneServices.Infrastructure.Services;
using PaymentTelephoneServices.Infrastructure.Services.MobileOperatorServices;

namespace PaymentTelephoneServices.Infrastructure.DependencyInjection;

internal static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var logger = services.BuildServiceProvider().GetRequiredService<ILogger<MobileOperatorServiceAggregatorOptions>>();
        services.AddSingleton(provider => new MobileOperatorServiceAggregatorOptions(configure => 
        {
            configure.Add("701", typeof(ActiveMobileOperatorService));
            configure.Add("777", typeof(BeelineMobileOperatorService));
            configure.Add("705", typeof(BeelineMobileOperatorService));
            configure.Add("707", typeof(TeleTwoMobileOperatorService));
            configure.Add("747", typeof(TeleTwoMobileOperatorService));
            configure.Add("700", typeof(AltelMobileOperatorService));
            configure.Add("708", typeof(AltelMobileOperatorService));
        }, logger));

        services.AddTransient<IMobileOperatorServicesAggregator, MobileOperatorServicesAggregator>();
        services.AddDbContext<TransactionsContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DbConnectionString")));

        services.AddTransient<IPaymentTransactionsDbService, PaymentTransactionsDbService>();

        return services;
    }
}
