using Microsoft.AspNetCore.Mvc;
using PaymentTelephoneServices.API.Models;
using PaymentTelephoneServices.Domain.Models;
using System.Text.Json;

namespace PaymentTelephoneServices.API.Controllers;

[Route("/Payment")]
public class PaymentController : Controller
{
    [HttpPost("SetPayment")]
    public async Task<IActionResult> SetPayment([FromBody] SetPaymentCommand request, CancellationToken cancellationToken)
    {
        Payment payment = new(request.PhoneNumber, request.PaymentAmount);
        return Ok(JsonSerializer.Serialize(payment));
    }
}
