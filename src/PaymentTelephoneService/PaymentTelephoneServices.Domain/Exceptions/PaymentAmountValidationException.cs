namespace PaymentTelephoneServices.Domain.Exceptions;

internal class PaymentAmountValidationException : ApplicationException
{
	public PaymentAmountValidationException() : base() { }
	public PaymentAmountValidationException(string message) : base(message) { }
}
