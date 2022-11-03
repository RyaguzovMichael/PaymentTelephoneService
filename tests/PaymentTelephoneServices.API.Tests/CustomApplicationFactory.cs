using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using PaymentTelephoneServices.API.Tests.Repository;
using PaymentTelephoneServices.Application.Contracts;
using PaymentTelephoneServices.Application.DependencyInjection;

namespace PaymentTelephoneServices.API.Tests;

[CollectionDefinition("WebApplicationFactory")]
public class CustomApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var repositoryDesc = services.First(s => s.ServiceType == typeof(IPaymentTransactionsDbService));
            services.Remove(repositoryDesc);
            services.AddTransient<IPaymentTransactionsDbService, MoqRepository>();

            var mobileOperatorServisceResolver = services.First(s => s.ServiceType == typeof(MobileOperatorServisceResolver));
            services.Remove(mobileOperatorServisceResolver);
            services.AddTransient<MoqMobileOperatorService>();
            services.AddTransient<MobileOperatorServisceResolver>(provider => operatorName =>
            {
                return provider.GetRequiredService<MoqMobileOperatorService>();
            });
        });
    }
}