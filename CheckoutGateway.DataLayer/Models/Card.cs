namespace CheckoutGateway.DataLayer.Models;

public class Card : Auditable
{
    public string CardNumber { get; set; }
    public string Cvv { get; set; }
    public string HolderName { get; set; }
    public string ExpiryMonth { get; set; }
    public string ExpiryYear { get; set; }
    public Customer Customer { get; set; }
    public CardStatus Status { get; set; }
}
