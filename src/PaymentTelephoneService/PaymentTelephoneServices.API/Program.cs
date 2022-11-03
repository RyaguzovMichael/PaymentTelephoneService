using Microsoft.AspNetCore.Mvc.Razor;
using PaymentTelephoneServices.API.DependencyInjection;
using PaymentTelephoneServices.API.Middlewares;
using PaymentTelephoneServices.API.Services;
using PaymentTelephoneServices.Application.DependencyInjection;
using PaymentTelephoneServices.Infrastructure.DependencyInjection;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.
services.AddLocalization(options => options.ResourcesPath = "Resources");
services.AddControllers().AddMvcLocalization(LanguageViewLocationExpanderFormat.Suffix);
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddSingleton<GlobalErrorsStringLocalizer>();
services.AddApiOptions(builder.Configuration);
services.AddSerilogLogging(builder.Logging);
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

public partial class Program { }