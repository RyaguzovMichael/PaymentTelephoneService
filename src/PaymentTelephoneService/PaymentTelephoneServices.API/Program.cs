using PaymentTelephoneServices.API.DependencyInjection;
using PaymentTelephoneServices.API.Middlewares;
using PaymentTelephoneServices.Application.Contracts;
using PaymentTelephoneServices.Application.DependencyInjection;
using PaymentTelephoneServices.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.
services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddApiOptions(builder.Configuration);
services.AddSerilogLogging(builder.Logging);
services.AddApplicationServices(builder.Configuration);
services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    try
    {
        var DbService = scope.ServiceProvider.GetRequiredService<IPaymentTransactionsDbService>();
        await DbService.SetMobileOperatorsData(new CancellationToken());
    }
    catch (Exception e)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(e, "An error occurred while seeding the database.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionHandler>();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();

public partial class Program { }