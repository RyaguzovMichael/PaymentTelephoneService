namespace PaymentTelephoneServices.Domain.Exceptions;

internal class MobileOperatorServiceIsNotPresented : ApplicationException
{
    public MobileOperatorServiceIsNotPresented() : base() { }
    public MobileOperatorServiceIsNotPresented(string message) : base(message) { }
}
