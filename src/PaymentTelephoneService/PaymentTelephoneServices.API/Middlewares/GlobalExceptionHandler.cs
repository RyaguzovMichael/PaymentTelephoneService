using PaymentTelephoneServices.API.Models;
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
        ResponseVm response;
        try
        {
            await _next.Invoke(context);
        }
        catch (PhoneValidationException ex)
        {
            _logger.LogError(ex.Message); 
            response = new ResponseVm()
            {
                IsSuccess = false,
                Error = "Введённый телефонный номер имеет неверный формат. Верный формат: +# (###) ### ## ##",
                ErrorCode = ErrorCodes.InvalidPhoneNumberInput,
                Message = null
            };
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            await context.Response.WriteAsJsonAsync(response);
        }
        catch (PaymentAmountValidationException ex)
        {
            _logger.LogError(ex.Message);
            response = new ResponseVm()
            {
                IsSuccess = false,
                Error = "Минимальная сумма пополнения равна 1 тенге",
                ErrorCode = ErrorCodes.InvalidPaymentAmountInput,
                Message = null
            };
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            await context.Response.WriteAsJsonAsync(response);
        }
        catch (NullReferenceException ex)
        {
            _logger.LogError("The argument is null or empty: " + ex.Message);
            response = new ResponseVm()
            {
                IsSuccess = false,
                Error = "Входящие данные оказались пусты",
                ErrorCode = ErrorCodes.InputDataIsEmpty,
                Message = null
            };
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            await context.Response.WriteAsJsonAsync(response);
        }
        catch (MobileOperatorServiceIsNotPresented ex)
        {
            _logger.LogError(ex.Message);
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            response = new ResponseVm()
            {
                IsSuccess = false,
                Error = "Мобильный оператор не поддерживается сервисом",
                ErrorCode = ErrorCodes.MobileOperatorIsNotSupported,
                Message = null
            };
            await context.Response.WriteAsJsonAsync(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsJsonAsync("Что то пошло не так");
        }
    }
}
