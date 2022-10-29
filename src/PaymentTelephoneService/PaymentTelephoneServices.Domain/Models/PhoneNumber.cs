using PaymentTelephoneServices.Domain.Exceptions;

namespace PaymentTelephoneServices.Domain.Models;

internal class PhoneNumber
{
    private const int MAXIMUM_INPUT_STRING_LENGTH = 22;
    private const int MINIMUM_INPUT_STRING_LENGTH = 11;

    public string CountryCode { get; init; }
    public string OperatorCode { get; init; }
    public string Number { get; init; }


    public PhoneNumber(string value)
    {
        if (string.IsNullOrEmpty(value))
            throw new ArgumentNullException(nameof(value));
        if (value.Length < MINIMUM_INPUT_STRING_LENGTH)
            throw new PhoneValidationException($"Inputed string can't convert to phone number. \n" +
                                               $"Reason: String length is so short, minimum length is: {MINIMUM_INPUT_STRING_LENGTH}\n" +
                                               $"The input value is: {value}");
        if (value.Length > MAXIMUM_INPUT_STRING_LENGTH)
            throw new PhoneValidationException($"Inputed string can't convert to phone number. \n" +
                                               $"Reason: So much string length, maximum length is: {MAXIMUM_INPUT_STRING_LENGTH}\n" +
                                               $"The input value is: {value}");
        value = value.Replace("(", "").Replace(")", "").Replace(" ", "");
        Number = value[^7..];
        if (!Number.All(@char => char.IsDigit(@char)))
            throw new PhoneValidationException($"Inputed string can't convert to phone number. \n" +
                                               $"Reason: Part of string \"{Number}\" is contain non-digit characters\n" +
                                               $"The input value is: {value}");
        OperatorCode = value[^10..^7];
        if (!OperatorCode.All(@char => char.IsDigit(@char)))
            throw new PhoneValidationException($"Inputed string can't convert to phone number. \n" +
                                               $"Reason: Part of string \"{OperatorCode}\" is contain non-digit characters\n" +
                                               $"The input value is: {value}");
        CountryCode = value[..^10];
    }

    public override string ToString()
    {
        return $"{CountryCode} ({OperatorCode}) {Number[0..3]} {Number[3..5]} {Number[5..7]}";
    }
}
