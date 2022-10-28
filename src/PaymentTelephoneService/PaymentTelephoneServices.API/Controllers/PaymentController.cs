using Microsoft.AspNetCore.Mvc;

namespace PaymentTelephoneServices.API.Controllers;

public class PaymentController : Controller
{
    public IActionResult SetPayment(string phoneNumber)
    {
        return View();
    }
}
