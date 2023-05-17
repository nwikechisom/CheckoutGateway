using AutoMapper;
using CheckoutGateway.BusinessLogic.Commands.RequestPayment;
using CheckoutGateway.BusinessLogic.Proxy.Bank.Models;
using CheckoutGateway.BusinessLogic.Proxy.Bank.Service;
using CheckoutGateway.BusinessLogic.Services.Caching;
using CheckoutGateway.DataLayer.Models;
using CheckoutGateway.DataLayer.Repositories;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;

namespace CheckoutGateway.Tests.CommandTests;

public class RequestPaymentCommandHandlerTests
{
    private Mock<ILogger<RequestPaymentCommandHandler>> _loggerMock;
    private Mock<IBankProxy> _bankProxyMock;
    private Mock<ICacheService> _cacheServiceMock;
    private Mock<IMapper> _mapperMock;
    private Mock<IValidator<RequestPaymentCommand>> _commandValidatorMock;
    private Mock<IGenericRepository<Transaction>> _transactionRepositoryMock;
    private RequestPaymentCommandHandler _commandHandler;

    
    public RequestPaymentCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<RequestPaymentCommandHandler>>();
        _bankProxyMock = new Mock<IBankProxy>();
        _cacheServiceMock = new Mock<ICacheService>();
        _mapperMock = new Mock<IMapper>();
        _commandValidatorMock = new Mock<IValidator<RequestPaymentCommand>>();
        _transactionRepositoryMock = new Mock<IGenericRepository<Transaction>>();

        _commandHandler = new RequestPaymentCommandHandler(
            _transactionRepositoryMock.Object,
            _loggerMock.Object,
            _bankProxyMock.Object,
            _cacheServiceMock.Object,
            _mapperMock.Object,
            _commandValidatorMock.Object
        );
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsRequestPaymentResponse()
    {
        // Arrange
        var request = new RequestPaymentCommand 
        {
            CardNumber= "4111111111111111",
          CardExpiryMonth= 12,
          CardExpiryYear=23,
          CardHolderName= "Stevie Wonder",
          CardCvv= "123",
          BillingAddress = new BillingAddress{
                    Address= "StationRoad",
            City="Midlesex",
            PostCode= "HA8 7El",
            Country= "United Kingdom"
          },
          Phone= new Phone {
                    CountryCode= "+44",
            Number = "07765642937"
          },
            Amount = 200,
            PaymentDescription = "Test",
            MerchantId = "36892619",
            Callback = "https://merchant.com/webhook",
            Reference = "23783993"
        }; // Create a valid command object

        _commandValidatorMock.Setup(v => v.Validate(request))
            .Returns(new ValidationResult());

        var verifyCardResponse = new BankResponse // Create a valid response from the bank proxy
        {
            Status = "00",
            Message = "Card validation successful",
            Reference = "BankReference"
        };
        _bankProxyMock.Setup(b => b.ValidateCard(
                request.CardNumber,
                request.CardExpiryMonth.ToString(),
                request.CardExpiryYear.ToString(),
                request.CardCvv,
                request.CardHolderName,
                request.Amount
            ))
            .ReturnsAsync(verifyCardResponse);

        _transactionRepositoryMock.Setup(r => r.Add(It.IsAny<Transaction>()));
        _transactionRepositoryMock.Setup(r => r.Save());
        _mapperMock.Setup(x => x.Map<Transaction>(It.IsAny<RequestPaymentCommand>())).Returns(new Transaction { });
        _cacheServiceMock.Setup(c => c.Set<string>(request.Reference, verifyCardResponse.Reference));

        var expectedResponse = new RequestPaymentResponse
        {
            TransactionReference = request.Reference,
            Message = verifyCardResponse.Message,
            Status = verifyCardResponse.Status
        };

        // Act
        var response = await _commandHandler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(expectedResponse.TransactionReference, response.TransactionReference);
        Assert.Equal(expectedResponse.Message, response.Message);
        Assert.Equal(expectedResponse.Status, response.Status);
    }

    [Fact]
    public async Task Handle_InvalidRequest_ReturnsInvalidRequestPaymentResponse()
    {
        // Arrange
        var request = new RequestPaymentCommand(); // Create an invalid command object
        var validationResult = new ValidationResult(new List<ValidationFailure>
        {
            new ValidationFailure("CardNumber", "Card number is required"),
            new ValidationFailure("CardExpiryMonth", "Card expiry month is required"),
            // Add more validation failures as needed
        });
        _commandValidatorMock.Setup(v => v.Validate(request))
            .Returns(validationResult);

        var expectedResponse = new RequestPaymentResponse
        {
            TransactionReference = request.Reference,
            Message = string.Join("\n", validationResult.Errors.Select(error => error.ErrorMessage)),
            Status = "50"
        };

        // Act
        var response = await _commandHandler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(expectedResponse.TransactionReference, response.TransactionReference);
    }
}
