namespace PaymentTelephoneServices.API.Models;

public enum ErrorCodes
{
    Success = 0,
    MobileOperatorServicesError,
    InvalidPhoneNumberInput,
    InvalidPaymentAmountInput,
    InputDataIsEmpty,
    MobileOperatorIsNotSupported
}
