using PaymentTelephoneServices.API.DependencyInjection;
using PaymentTelephoneServices.API.Middlewares;
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