namespace PaymentTelephoneServices.Domain.DbModels;

internal class Operator
{
    public int Id { get; set; }
    public string Name { get; init; }
    public IEnumerable<Transaction> Transactions { get; set; }
}
