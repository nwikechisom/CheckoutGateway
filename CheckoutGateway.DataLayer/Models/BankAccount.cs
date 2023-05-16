namespace CheckoutGateway.DataLayer.Models;

public class BankAccount : Auditable
{
    public Guid MerchantId { get; set; }
    public Guid BankId { get; set; }
    public string AccountName { get; set; }
    public string AccountNumber { get; set; }
}
