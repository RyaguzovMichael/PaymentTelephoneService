namespace PaymentTelephoneServices.Domain.OptionModels;

internal class OperatorCodesOptions
{
    public IEnumerable<OperatorCodes> OperatorCodes { get; set; }

    public string? GetOperatorNameByCode(string code)
    {
        foreach (var item in OperatorCodes)
        {
            if(item.Codes.Any(c => c == code))
            {
                return item.OperatorName;
            }
        }
        return null;
    }
}
