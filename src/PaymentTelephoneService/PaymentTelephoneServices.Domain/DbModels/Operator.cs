namespace PaymentTelephoneServices.Domain.DbModels;

internal class Operator
{
    public int Id { get; set; }
    public string Name { get; set; }
    public IEnumerable<Transaction> Transactions { get; set; }
}
