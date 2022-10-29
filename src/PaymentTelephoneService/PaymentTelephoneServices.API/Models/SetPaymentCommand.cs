namespace PaymentTelephoneServices.API.Models;

public class SetPaymentCommand
{
    public string PhoneNumber { get; set; }
    public decimal PaymentAmount { get; set; }
}
