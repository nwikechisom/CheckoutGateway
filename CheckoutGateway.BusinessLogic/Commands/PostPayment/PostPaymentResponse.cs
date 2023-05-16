using CheckoutGateway.BusinessLogic.Commands.RequestPayment;

namespace CheckoutGateway.BusinessLogic.Commands.PostPayment;

public class PostPaymentResponse
{
    public bool Posted { get; set; }
    public double Charge { get; set; }
    public string Currency { get; set; }
    public string Reference { get; set; }
    public string Description { get; set; }
    public BillingAddress Billing { get; set; }
}
