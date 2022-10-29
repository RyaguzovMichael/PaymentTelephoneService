using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaymentTelephoneServices.API.Models;
using PaymentTelephoneServices.Application.Features.SetPayment;
using PaymentTelephoneServices.Domain.Models;

namespace PaymentTelephoneServices.API.Controllers;

[Route("/Payment")]
public class PaymentController : Controller
{
    private readonly IMediator _mediator;

    public PaymentController(IMediator mediator)
    {
        _mediator = mediator;
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
                IsSucsess = true,
                Error = null,
                ErrorCode = ErrorCodes.Sucsess,
                Message = "Ваш запрос был успешно выполнен"
            };
            return Ok(response);
        }
        response = new ResponseVm()
        {
            IsSucsess = false,
            Error = "Ваш запрос не был обработан по техническим причинам, повторите позже",
            ErrorCode = ErrorCodes.MobileOperatorServisesError,
            Message = null
        };
        return Ok(response);
    }
}
