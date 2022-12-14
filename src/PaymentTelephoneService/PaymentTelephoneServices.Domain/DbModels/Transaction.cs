using System.Text.Json.Serialization;

namespace PaymentTelephoneServices.Domain.DbModels;

internal class Transaction
{
    public int Id { get; set; }
    [JsonIgnore]
    public int OperatorId { get; set; }
    public Operator Operator { get; set; }
    [JsonIgnore]
    public int UserId { get; set; }
    public User User { get; set; }
    public decimal PaymentAmount { get; init; }
    public DateTime TransactionTime { get; init; }
}
