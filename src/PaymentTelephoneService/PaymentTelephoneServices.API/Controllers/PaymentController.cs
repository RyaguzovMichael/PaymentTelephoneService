using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using PaymentTelephoneServices.API.Models;
using PaymentTelephoneServices.Application.Features.SetPayment;
using PaymentTelephoneServices.Domain.Models;

namespace PaymentTelephoneServices.API.Controllers;

[Route("/Payment")]
public class PaymentController : Controller
{
    private readonly IMediator _mediator;
    private readonly IStringLocalizer<PaymentController> _localizer;

    public PaymentController(IMediator mediator, IStringLocalizer<PaymentController> localizer)
    {
        _mediator = mediator;
        _localizer = localizer;
    }

    [HttpPost("SetPayment")]
    public async Task<ActionResult<ResponseVm>> SetPayment([FromBody] SetPaymentControllerCommand request, CancellationToken cancellationToken)
    {
        Payment payment = new(request.PhoneNumber, request.PaymentAmount);
        SetPaymentCommand command = new() { Payment = payment };
        ResponseVm response;
        if (await _mediator.Send(command, cancellationToken))
        {
            response = new ResponseVm()
            {
                IsSuccess = true,
                Error = null,
                ErrorCode = ErrorCodes.Success,
                Message = _localizer["SuccessPayment"] 
            };
            return Ok(response);
        }
        response = new ResponseVm()
        {
            IsSuccess = false,
            Error = _localizer["ErrorPayment"],
            ErrorCode = ErrorCodes.MobileOperatorServicesError,
            Message = null
        };
        return Ok(response);
    }
}
