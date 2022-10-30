namespace PaymentTelephoneServices.Domain.DbModels;

internal class User
{
    public int Id { get; set; }
    public string PhoneNumber { get; init; }
    public IEnumerable<Transaction> Transactions { get; set; }
}
