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
    public async Task<IActionResult> SetPayment([FromBody] SetPaymentControllerCommand request, CancellationToken cancellationToken)
    {
        Payment payment = new(request.PhoneNumber, request.PaymentAmount);
        SetPaymentCommand command = new() { Payment = payment };
        if (await _mediator.Send(command, cancellationToken))
        {
            return Ok("Ваш запрос был успешно выполнен");
        }
        return Ok("Ваш запрос не был обработан по техническим причинам, повторите позже");
    }
}
