using AutoMapper;
using CheckoutGateway.BusinessLogic.Proxy.Bank.Service;
using CheckoutGateway.BusinessLogic.Services.Caching;
using CheckoutGateway.DataLayer.Models;
using CheckoutGateway.DataLayer.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CheckoutGateway.BusinessLogic.Commands.RequestPayment;

public class RequestPaymentCommandHandler : IRequestHandler<RequestPaymentCommand, RequestPaymentResponse>
{
    private readonly ILogger<RequestPaymentCommandHandler> _logger;
    private readonly IBankProxy _bankProxy;
    private readonly ICacheService _cacheService;
    private readonly IMapper _mapper;
    private readonly IValidator<RequestPaymentCommand> _comandValidator;
    private readonly IGenericRepository<Transaction> _transactionRepository;

    public RequestPaymentCommandHandler(IGenericRepository<Transaction> transactionRepository, 
        ILogger<RequestPaymentCommandHandler> logger, IBankProxy bankProxy,
        ICacheService cacheService, IMapper mapper, IValidator<RequestPaymentCommand> comandValidator)
    {
        _transactionRepository = transactionRepository;
        _logger = logger;
        _bankProxy = bankProxy;
        _cacheService = cacheService;
        _mapper = mapper;
        _comandValidator = comandValidator;
    }
    public async Task<RequestPaymentResponse> Handle(RequestPaymentCommand request, CancellationToken cancellationToken)
    {
        //validate request model
        var validationResult = _comandValidator.Validate(request);
        if (validationResult.Errors.Any())
        {
            var errors = validationResult.Errors.Select(error => error.ErrorMessage);
            return new RequestPaymentResponse
            {
                TransactionReference = request.Reference,
                Message = string.Join("\n", errors),
                Status = "50"
            };
        }
        //map request to transaction DB
        var transaction = _mapper.Map<Transaction>(request);

        //send request to issuing bank to validate card
        var verifyCardResponse = await _bankProxy.ValidateCard(request.CardNumber, request.CardExpiryMonth.ToString(), request.CardExpiryYear.ToString(), request.CardCvv, request.CardHolderName, request.Amount);
        if (verifyCardResponse is null || verifyCardResponse.Status != "00")
        {
            //save transaction attempt to DB with Invalid status
            transaction.Status = TransactionStatus.InvalidDetails;
            _transactionRepository.Add(transaction);
            _transactionRepository.Save();
            
            return new RequestPaymentResponse
            { 
                TransactionReference = request.Reference,
                Message = verifyCardResponse?.Message ?? "Unable to validate card information",
                Status = "99"
            };
        }
        // cache reference gotten from reponse with transcation request reference
        _cacheService.Set<string>(request.Reference, verifyCardResponse.Reference);

        //Update Transaction in DB
        transaction.Status = TransactionStatus.Pending;
        _transactionRepository.Add(transaction);
        _transactionRepository.Save();

        //return response
        return new RequestPaymentResponse
        {
            TransactionReference = request.Reference,
            Message = verifyCardResponse.Message,
            Status = "00"
        };
    }
}
