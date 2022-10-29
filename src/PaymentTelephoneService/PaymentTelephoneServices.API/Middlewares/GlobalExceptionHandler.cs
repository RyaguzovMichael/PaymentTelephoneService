using PaymentTelephoneServices.Domain.Exceptions;
using System.Net;

namespace PaymentTelephoneServices.API.Middlewares;

internal class GlobalExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly RequestDelegate _next;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, RequestDelegate next)
    {
        _logger = logger;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (PhoneValidationException ex)
        {
            _logger.LogError(ex.Message);
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsJsonAsync("Введённый телефонный номер имеет неверный формат. Верный формат: +# (###) ### ## ##");
        }
        catch (PaymentAmountValidationException ex)
        {
            _logger.LogError(ex.Message);
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsJsonAsync("Минимальная сумма пополнения равна 1 тенге");
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError("The argument is null or empty: " + ex.Message);
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsJsonAsync("Входящие данные оказались пусты");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsJsonAsync("Что то пошло не так");
        }
    }
}
