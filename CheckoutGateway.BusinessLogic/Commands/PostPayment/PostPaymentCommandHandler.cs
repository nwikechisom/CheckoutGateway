using CheckoutGateway.BusinessLogic.Proxy.Bank.Service;
using CheckoutGateway.BusinessLogic.Services.Caching;
using CheckoutGateway.DataLayer.Models;
using CheckoutGateway.DataLayer.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CheckoutGateway.BusinessLogic.Commands.PostPayment;

public class PostPaymentCommandHandler : IRequestHandler<PostPaymentCommand, PostPaymentResponse>
{
    private readonly IBankProxy _bankProxy;
    private readonly IGenericRepository<Transaction> _transactionRepository;
    private readonly ILogger<PostPaymentCommandHandler> _logger;
    private readonly ICacheService _cacheService;

    public PostPaymentCommandHandler(IBankProxy bankProxy, IGenericRepository<Transaction> transactionRepository, 
        ILogger<PostPaymentCommandHandler> logger, ICacheService cacheService)
    {
        _bankProxy = bankProxy;
        _transactionRepository = transactionRepository;
        _logger = logger;
        _cacheService = cacheService;
    }
    public async Task<PostPaymentResponse> Handle(PostPaymentCommand request, CancellationToken cancellationToken)
    {
        var transaction = _transactionRepository.Find(t => t.Reference == request.TransactionReference).First();
        if (transaction != null)
        {
            var cachedBankReference = _cacheService.Get<string>(request.TransactionReference);
            var authorizeProcess = await _bankProxy.ProcessTransaction(cachedBankReference, request.OneTimePassword);
            if (authorizeProcess != null)
            {
                _transactionRepository.Update(transaction);
                _transactionRepository.Save();
                return new PostPaymentResponse { Posted = true };
            }
        }
        
        return new PostPaymentResponse { Posted = false };
    }
}
