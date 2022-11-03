using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PaymentTelephoneServices.Application.Contracts;
using System.Reflection;

namespace PaymentTelephoneServices.Application.DependencyInjection;

internal delegate IMobileOperatorService MobileOperatorServisceResolver(string operatorName);

internal static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());

        return services;
    }
}
