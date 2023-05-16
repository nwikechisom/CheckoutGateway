namespace CheckoutGateway.DataLayer.Models;

public class Transaction : Auditable
{
    public double Amount { get; set; }
    public double Charge { get; set; }
    public string Merchant { get; set; }
    public string Currency { get; set; }
    public Customer Customer { get; set; }
    public string CallBackUrl { get; set; }
    public string Reference { get; set; }
    public TransactionStatus Status { get; set; }
}
