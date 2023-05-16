using MediatR;

namespace CheckoutGateway.BusinessLogic.Commands.RequestPayment;

public class RequestPaymentCommand : IRequest<RequestPaymentResponse>
{
    public string CardNumber { get; set; }
    public int CardExpiryMonth { get; set; }
    public int CardExpiryYear { get; set; }
    public string CardHolderName { get; set; }
    public string CardCvv { get; set; }
    public BillingAddress BillingAddress { get; set; }
    public Phone Phone { get; set; }
    public string Reference { get; set; }
    public double Amount { get; set; }
    public string PaymentDescription { get; set; }
    public string MerchantId { get; set; }
    public string Callback { get; set; }
    public string Currency { get; set; }
}

public class BillingAddress
{
    public string Address { get; set; }
    public string City { get; set; }
    public string PostCode { get; set; }
    public string Country { get; set; }
}

public class Phone
{
    public string CountryCode { get; set; }
    public string Number { get; set; }
}
