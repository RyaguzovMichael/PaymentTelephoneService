using Microsoft.AspNetCore.Mvc.Razor;
using NLog;
using NLog.Web;
using PaymentTelephoneServices.API.DependencyInjection;
using PaymentTelephoneServices.API.Middlewares;
using PaymentTelephoneServices.API.Services;
using PaymentTelephoneServices.Application.DependencyInjection;
using PaymentTelephoneServices.Infrastructure.DependencyInjection;
using System.Globalization;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.AddLogging();
    var services = builder.Services;

    // Add services to the container.
    services.AddLocalization(options => options.ResourcesPath = "Resources");
    services.AddControllers().AddMvcLocalization(LanguageViewLocationExpanderFormat.Suffix);
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
    services.AddSingleton<GlobalErrorsStringLocalizer>();
    services.AddApiOptions(builder.Configuration);
    services.AddApplicationServices();
    services.AddInfrastructureServices(builder.Configuration);

    var app = builder.Build();

    var supportedCultures = new[]
    {
    new CultureInfo("kk"),
    new CultureInfo("ru")
};
    app.UseRequestLocalization(new RequestLocalizationOptions
    {
        SupportedCultures = supportedCultures,
        SupportedUICultures = supportedCultures
    });

    app.UseMiddleware<GlobalExceptionHandler>();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.MapControllers();


    app.Run();
}
catch (Exception exception)
{
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    LogManager.Shutdown();
}

public partial class Program { }