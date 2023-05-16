namespace CheckoutGateway.DataLayer.Models;

public class Currency : BaseModel
{
    public string Code { get; set; }
    public bool IsActive { get; set; }
}
