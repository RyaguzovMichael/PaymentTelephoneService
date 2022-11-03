using NLog.Web;
using PaymentTelephoneServices.Domain.OptionModels;

namespace PaymentTelephoneServices.API.DependencyInjection;

internal static class DependencyInjection
{
    public static IServiceCollection AddApiOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OperatorCodesOptions>(configuration.GetSection("OperatorCodesOptions"));

        return services;
    }

    public static WebApplicationBuilder AddLogging(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        builder.Host.UseNLog();

        return builder;
    }
}
