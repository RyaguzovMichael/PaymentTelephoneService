namespace PaymentTelephoneServices.Domain.OptionModels;

internal class OperatorCodes
{
    public string OperatorName { get; set; }
    public IEnumerable<string> Codes { get; set; }
}
