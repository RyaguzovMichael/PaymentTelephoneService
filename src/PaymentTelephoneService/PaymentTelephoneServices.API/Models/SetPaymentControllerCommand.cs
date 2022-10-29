namespace PaymentTelephoneServices.API.Models
{
    public class SetPaymentControllerCommand
    {
        public string PhoneNumber { get; set; }
        public decimal PaymentAmount { get; set; }
    }
}