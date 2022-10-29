using PaymentTelephoneServices.Domain.OptionModels;
using Serilog;

namespace PaymentTelephoneServices.API.DependencyInjection;

internal static class DependencyInjection
{
    public static IServiceCollection AddApiOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OperatorCodesOptions>(configuration.GetSection("OperatorCodesOptions"));

        return services;
    }

    public static IServiceCollection AddSerilogLogging(this IServiceCollection services, ILoggingBuilder loggingBuilder)
    {
        string logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "logs", "application.log");
        var logger = new LoggerConfiguration().WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day).CreateLogger();
        loggingBuilder.AddSerilog(logger);

        return services;
    }
}
