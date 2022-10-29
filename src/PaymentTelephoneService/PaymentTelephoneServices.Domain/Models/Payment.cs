using PaymentTelephoneServices.Domain.Exceptions;

namespace PaymentTelephoneServices.Domain.Models;

internal class Payment
{
    public PhoneNumber PhoneNumber { get; set; }
    public decimal PaymentAmount { get; set; }

    public Payment(string phoneNumber, decimal paymentAmount)
    {
        if (string.IsNullOrEmpty(phoneNumber))
            throw new NullReferenceException(nameof(phoneNumber));
        if (paymentAmount < 1m)
            throw new PaymentAmountValidationException($"The inputed payment amount is less then 1. \n" +
                                                 $"Inputed value is: {paymentAmount}");
        PhoneNumber = new(phoneNumber);
        PaymentAmount = paymentAmount;
    }
}
