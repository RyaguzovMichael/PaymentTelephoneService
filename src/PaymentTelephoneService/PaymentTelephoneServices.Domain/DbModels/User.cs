using System.Text.Json.Serialization;

namespace PaymentTelephoneServices.Domain.DbModels;

internal class User
{
    public int Id { get; set; }
    public string PhoneNumber { get; init; }
    [JsonIgnore]
    public IEnumerable<Transaction> Transactions { get; set; }
}
