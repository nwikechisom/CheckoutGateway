using MediatR;

namespace CheckoutGateway.BusinessLogic.Queries.PaymentDetails;

public class PaymentDetailQuery : IRequest<PaymentDetailQueryResponse>
{
    public string TransactionReference { get; set; }
}
