using CheckoutGateway.BusinessLogic.Commands.RequestPayment;

namespace CheckoutGateway.BusinessLogic.Queries.PaymentDetails;

public class PaymentDetailQueryResponse
{
    public double Charge { get; set; }
    public string Currency { get; set; }
    public string Reference { get; set; }
    public string Description { get; set; }
    public BillingAddress Billing { get; set; }
    public string Phone { get; set; }
    public string Status { get; set; }
}
