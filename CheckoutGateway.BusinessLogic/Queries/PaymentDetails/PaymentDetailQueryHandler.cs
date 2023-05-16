using AutoMapper;
using CheckoutGateway.DataLayer.Models;
using CheckoutGateway.DataLayer.Repositories;
using MediatR;

namespace CheckoutGateway.BusinessLogic.Queries.PaymentDetails;

public class PaymentDetailQueryHandler : IRequestHandler<PaymentDetailQuery, PaymentDetailQueryResponse>
{
    private IGenericRepository<Transaction> _transactionRepository;
    private readonly IMapper _mapper;

    public PaymentDetailQueryHandler(IGenericRepository<Transaction> transactionRepository, IMapper mapper)
    {
        _transactionRepository = transactionRepository;
        _mapper = mapper;
    }
    public async Task<PaymentDetailQueryResponse> Handle(PaymentDetailQuery request, CancellationToken cancellationToken)
    {
        var transaction = _transactionRepository.Find(t => t.Reference == request.TransactionReference, t => t.Customer);
        if(transaction != null)
        {
            var response = _mapper.Map<PaymentDetailQueryResponse>(transaction);
            return response;
        }
        return new PaymentDetailQueryResponse { Description = "Invalid transaction Reference", Reference = request.TransactionReference};
    }
}
