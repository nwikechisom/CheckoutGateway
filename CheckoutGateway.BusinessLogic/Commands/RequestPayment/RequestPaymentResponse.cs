namespace CheckoutGateway.BusinessLogic.Commands.RequestPayment;

public class RequestPaymentResponse
{
    public string TransactionReference { get; set; }
    public string Message { get; set; }
    public string Status { get; set; }
}
