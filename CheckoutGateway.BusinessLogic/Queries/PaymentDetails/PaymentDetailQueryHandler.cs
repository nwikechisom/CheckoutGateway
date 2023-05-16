using MediatR;

namespace CheckoutGateway.BusinessLogic.Queries.PaymentDetails;

public class PaymentDetailQueryHandler : IRequestHandler<PaymentDetailQuery, PaymentDetailQueryResponse>
{
    public PaymentDetailQueryHandler()
    {
        
    }
    Task<PaymentDetailQueryResponse> IRequestHandler<PaymentDetailQuery, PaymentDetailQueryResponse>.Handle(PaymentDetailQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
