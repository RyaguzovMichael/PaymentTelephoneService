using PaymentTelephoneServices.API.Models;
using PaymentTelephoneServices.Domain.Exceptions;
using System.Net;
using PaymentTelephoneServices.API.Services;

namespace PaymentTelephoneServices.API.Middlewares;

internal class GlobalExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly RequestDelegate _next;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, 
                                  RequestDelegate next 
                                  )
    {
        _logger = logger;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, GlobalErrorsStringLocalizer localizer)
    {
        ResponseVm response;
        try
        {
            await _next.Invoke(context);
        }
        catch (PhoneValidationException ex)
        {
            _logger.LogWarning(ex.Message); 
            response = new ResponseVm()
            {
                IsSuccess = false,
                Error = localizer["InvalidPhoneNumberInput"],
                ErrorCode = ErrorCodes.InvalidPhoneNumberInput,
                Message = null
            };
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            await context.Response.WriteAsJsonAsync(response);
        }
        catch (PaymentAmountValidationException ex)
        {
            _logger.LogWarning(ex.Message);
            response = new ResponseVm()
            {
                IsSuccess = false,
                Error = localizer["InvalidPaymentAmountInput"],
                ErrorCode = ErrorCodes.InvalidPaymentAmountInput,
                Message = null
            };
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            await context.Response.WriteAsJsonAsync(response);
        }
        catch (NullReferenceException ex)
        {
            _logger.LogWarning("The argument is null or empty: " + ex.Message);
            response = new ResponseVm()
            {
                IsSuccess = false,
                Error = localizer["InputDataIsEmpty"],
                ErrorCode = ErrorCodes.InputDataIsEmpty,
                Message = null
            };
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            await context.Response.WriteAsJsonAsync(response);
        }
        catch (MobileOperatorServiceIsNotPresented ex)
        {
            _logger.LogWarning(ex.Message);
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            response = new ResponseVm()
            {
                IsSuccess = false,
                Error = localizer["MobileOperatorIsNotSupported"],
                ErrorCode = ErrorCodes.MobileOperatorIsNotSupported,
                Message = null
            };
            await context.Response.WriteAsJsonAsync(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsJsonAsync(localizer["Error500"]);
        }
    }
}
