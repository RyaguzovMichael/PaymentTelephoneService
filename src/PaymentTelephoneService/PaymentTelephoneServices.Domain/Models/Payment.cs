using PaymentTelephoneServices.Domain.Exceptions;

namespace PaymentTelephoneServices.Domain.Models;

internal class Payment
{
    public PhoneNumber PhoneNumber { get; }
    public decimal PaymentAmount { get; }

    public Payment(string phoneNumber, decimal paymentAmount)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new NullReferenceException(nameof(phoneNumber));
        if (paymentAmount < 1m)
            throw new PaymentAmountValidationException($"The inputted payment amount is less then 1. \n" +
                                                 $"Inputted value is: {paymentAmount}");
        PhoneNumber = new PhoneNumber(phoneNumber);
        PaymentAmount = paymentAmount;
    }
}
