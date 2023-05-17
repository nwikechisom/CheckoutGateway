using AutoMapper;
using CheckoutGateway.BusinessLogic.Proxy.Bank.Service;
using CheckoutGateway.BusinessLogic.Services.Caching;
using CheckoutGateway.DataLayer.Models;
using CheckoutGateway.DataLayer.Repositories;
using MediatR;

namespace CheckoutGateway.BusinessLogic.Commands.PostPayment;

public class PostPaymentCommandHandler : IRequestHandler<PostPaymentCommand, PostPaymentResponse>
{
    private readonly IBankProxy _bankProxy;
    private readonly IGenericRepository<Transaction> _transactionRepository;
    private readonly ICacheService _cacheService;
    private readonly IMapper _mapper;

    public PostPaymentCommandHandler(IBankProxy bankProxy, 
        IGenericRepository<Transaction> transactionRepository,
        ICacheService cacheService, IMapper mapper)
    {
        _bankProxy = bankProxy;
        _transactionRepository = transactionRepository;
        _cacheService = cacheService;
        _mapper = mapper;
    }
    public async Task<PostPaymentResponse> Handle(PostPaymentCommand request, CancellationToken cancellationToken)
    {
        var transaction = _transactionRepository.Find(t => t.Reference == request.TransactionReference && t.Merchant == request.MerchantId, x => x.Customer).First();
        {
            var cachedBankReference = _cacheService.Get<string>(request.TransactionReference);
            var processTransaction = await _bankProxy.ProcessTransaction(cachedBankReference, request.OneTimePassword);
            if (processTransaction != null && processTransaction.Status == "00")
            {
                transaction.Status = TransactionStatus.Success;
                _transactionRepository.Update(transaction);
                _transactionRepository.Save();
                var response = _mapper.Map<PostPaymentResponse>(transaction);
                response.Posted = true;
                return response;
            }

            transaction.Status = TransactionStatus.Failed;
            _transactionRepository.Update(transaction);
            _transactionRepository.Save();
        }
        
        return new PostPaymentResponse { Posted = false, Reference = request.TransactionReference, Description = "Invalid Transaction Reference" };
    }
}
