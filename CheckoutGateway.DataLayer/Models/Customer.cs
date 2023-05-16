namespace CheckoutGateway.DataLayer.Models;

public class Customer : Auditable
{
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string PostCode { get; set; }
    public string Country { get; set; }
    public double TotalBalance { get; set; }
    public double Lien { get; set; } = 0;
    public double AvailableBalance => TotalBalance - Lien;

}