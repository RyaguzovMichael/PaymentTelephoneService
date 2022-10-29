namespace PaymentTelephoneServices.Domain.DbModels;

internal class User
{
    public int Id { get; set; }
    public string PhoneNumber { get; set; }
    public IEnumerable<Transaction> Transactions { get; set; }
}
