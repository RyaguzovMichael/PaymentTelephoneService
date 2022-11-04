namespace PaymentTelephoneServices.API.Models
{
    public class SetPaymentControllerCommand
    {
        public string PhoneNumber { get; init; }
        public decimal PaymentAmount { get; init; }
    }
}