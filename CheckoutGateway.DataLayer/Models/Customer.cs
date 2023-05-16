namespace CheckoutGateway.DataLayer.Models;

public class Customer : Auditable
{
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Name { get; set; }
    public double TotalBalance { get; set; }
    public double Lien { get; set; } = 0;
    public double AvailableBalance => TotalBalance - Lien;

}