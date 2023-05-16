namespace CheckoutGateway.DataLayer.Models;
public class Merchant : Auditable
{
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsVerified { get; set; } //Verification determines transaction amount limit
    public double Balance { get; set; }
    public string MerchantUniqueId { get; set; }
}
