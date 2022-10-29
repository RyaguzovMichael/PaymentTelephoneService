namespace PaymentTelephoneServices.Domain.Exceptions;

internal class PhoneValidationException : ApplicationException
{
	public PhoneValidationException() : base() { }
	public PhoneValidationException(string message) : base(message) { }
}
