using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PaymentTelephoneServices.API.Tests.Repository;
using PaymentTelephoneServices.Application.Contracts;
using PaymentTelephoneServices.Infrastructure.DependencyInjection.Options;

namespace PaymentTelephoneServices.API.Tests;

[CollectionDefinition("WebApplicationFactory")]
public class CustomApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var repositoryDesc = services.FirstOrDefault(s => s.ServiceType == typeof(IPaymentTransactionsDbService));
            services.Remove(repositoryDesc);
            services.AddTransient<IPaymentTransactionsDbService, MoqRepository>();
            var mobileOperatorServicesAggregatorOptions = services.FirstOrDefault(s => s.ServiceType == typeof(MobileOperatorServiceAggregatorOptions));
            services.Remove(mobileOperatorServicesAggregatorOptions);
            var logger = services.BuildServiceProvider().GetService<ILogger<MobileOperatorServiceAggregatorOptions>>();
            services.AddSingleton(provider => new MobileOperatorServiceAggregatorOptions(configuration =>
            {
                configuration.Add("701", typeof(MoqMobileOperatorService));
                configuration.Add("777", typeof(MoqMobileOperatorService));
                configuration.Add("705", typeof(MoqMobileOperatorService));
                configuration.Add("707", typeof(MoqMobileOperatorService));
                configuration.Add("747", typeof(MoqMobileOperatorService));
                configuration.Add("700", typeof(MoqMobileOperatorService));
                configuration.Add("708", typeof(MoqMobileOperatorService));
            }, logger));
        });
    }
}