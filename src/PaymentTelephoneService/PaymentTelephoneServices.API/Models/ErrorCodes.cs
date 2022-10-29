namespace PaymentTelephoneServices.API.Models;

public enum ErrorCodes
{
    Sucsess = 0,
    MobileOperatorServisesError,
    InvalidPhoneNumberInput,
    InvalidPaymentAmountInput,
    InputDataIsEmpty,
    MobileOperatorIsNotSupported
}
