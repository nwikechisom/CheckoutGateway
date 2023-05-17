using AutoMapper;
using CheckoutGateway.BusinessLogic.Commands.RequestPayment;
using CheckoutGateway.BusinessLogic.Proxy.Bank.Models;
using CheckoutGateway.BusinessLogic.Proxy.Bank.Service;
using CheckoutGateway.BusinessLogic.Services.Caching;
using CheckoutGateway.DataLayer.Models;
using CheckoutGateway.DataLayer.Repositories;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;

namespace CheckoutGateway.Tests.CommandTests;

public class RequestPaymentCommandHandlerTest_Extended
{
    private readonly Mock<ILogger<RequestPaymentCommandHandler>> _loggerMock;
    private readonly Mock<IBankProxy> _bankProxyMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IValidator<RequestPaymentCommand>> _commandValidatorMock;
    private readonly Mock<IGenericRepository<Transaction>> _transactionRepositoryMock;

    private readonly RequestPaymentCommandHandler _handler;

    public RequestPaymentCommandHandlerTest_Extended()
    {
        _loggerMock = new Mock<ILogger<RequestPaymentCommandHandler>>();
        _bankProxyMock = new Mock<IBankProxy>();
        _cacheServiceMock = new Mock<ICacheService>();
        _mapperMock = new Mock<IMapper>();
        _commandValidatorMock = new Mock<IValidator<RequestPaymentCommand>>();
        _transactionRepositoryMock = new Mock<IGenericRepository<Transaction>>();

        _handler = new RequestPaymentCommandHandler(
            _transactionRepositoryMock.Object,
            _loggerMock.Object,
            _bankProxyMock.Object,
            _cacheServiceMock.Object,
            _mapperMock.Object,
            _commandValidatorMock.Object
        );
    }

    [Fact]
    public async Task Handle_WithValidRequest_ShouldReturnSuccessfulResponse()
    {
        // Arrange
        var request = new RequestPaymentCommand
        {
            CardNumber = "4111111111111111",
            CardExpiryMonth = 12,
            CardExpiryYear = 23,
            CardHolderName = "Stevie Wonder",
            CardCvv = "123",
            BillingAddress = new BillingAddress
            {
                Address = "StationRoad",
                City = "Midlesex",
                PostCode = "HA8 7El",
                Country = "United Kingdom"
            },
            Phone = new Phone
            {
                CountryCode = "+44",
                Number = "07765642937"
            },
            Amount = 200,
            PaymentDescription = "Test",
            MerchantId = "36892619",
            Callback = "https://merchant.com/webhook",
            Reference = "23783993"
        };

        var validationResult = new FluentValidation.Results.ValidationResult();
        _commandValidatorMock.Setup(x => x.Validate(request)).Returns(validationResult);

        var verifyCardResponse = new BankResponse
        {
            // Setting up verify card response properties here
            Status = "00",
            Message = "Card validation successful",
            Reference = "BankReference"
        };
        _bankProxyMock.Setup(x => x.ValidateCard(
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<double>()
        )).ReturnsAsync(verifyCardResponse);

        var transaction = new Transaction();
        _mapperMock.Setup(x => x.Map<Transaction>(request)).Returns(transaction);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        response.Should().NotBeNull();
        response.Status.Should().Be("00");
        response.TransactionReference.Should().Be(request.Reference);
        response.Message.Should().Be(verifyCardResponse.Message);

        _commandValidatorMock.Verify(x => x.Validate(request), Times.Once);
        _bankProxyMock.Verify(x => x.ValidateCard(
            request.CardNumber, request.CardExpiryMonth.ToString(), request.CardExpiryYear.ToString(),
            request.CardCvv, request.CardHolderName, request.Amount
        ), Times.Once);

        _transactionRepositoryMock.Verify(x => x.Add(transaction), Times.Once);
        _transactionRepositoryMock.Verify(x => x.Save(), Times.Once);

        _cacheServiceMock.Verify(x => x.Set<string>(request.Reference, verifyCardResponse.Reference), Times.Once);
    }

    [Fact]
    public async Task Handle_WithInvalidRequest_ShouldReturnErrorResponse()
    {
        // Arrange
        var request = new RequestPaymentCommand
        {
            // Setting up invalid request properties here
        };
        var validationResult = new FluentValidation.Results.ValidationResult();
        validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure("PropertyName", "Error message"));
        _commandValidatorMock.Setup(x => x.Validate(request)).Returns(validationResult);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        response.Should().NotBeNull();
        response.Status.Should().Be("50");
        response.TransactionReference.Should().Be(request.Reference);
        response.Message.Should().Contain("Error message");

        _commandValidatorMock.Verify(x => x.Validate(request), Times.Once);
        _bankProxyMock.Verify(x => x.ValidateCard(
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<double>()
        ), Times.Never);

        _transactionRepositoryMock.Verify(x => x.Add(It.IsAny<Transaction>()), Times.Never);
        _transactionRepositoryMock.Verify(x => x.Save(), Times.Never);

        _cacheServiceMock.Verify(x => x.Set<string>(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithInvalidCardDetails_ShouldReturnErrorResponse()
    {
        // Arrange
        var request = new RequestPaymentCommand
        {
            // Setting up your request properties here
        };

        var validationResult = new FluentValidation.Results.ValidationResult();
        _commandValidatorMock.Setup(x => x.Validate(request)).Returns(validationResult);

        var verifyCardResponse = new BankResponse
        {
            Status = "99",
            Message = "Invalid card details"
        };
        _bankProxyMock.Setup(x => x.ValidateCard(
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<double>()
        )).ReturnsAsync(verifyCardResponse);

        var transaction = new Transaction();
        _mapperMock.Setup(x => x.Map<Transaction>(request)).Returns(transaction);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        response.Should().NotBeNull();
        response.Status.Should().Be("99");
        response.TransactionReference.Should().Be(request.Reference);
        response.Message.Should().Be("Invalid card details");

        _commandValidatorMock.Verify(x => x.Validate(request), Times.Once);
        _bankProxyMock.Verify(x => x.ValidateCard(
            request.CardNumber, request.CardExpiryMonth.ToString(), request.CardExpiryYear.ToString(),
            request.CardCvv, request.CardHolderName, request.Amount
        ), Times.Once);

        _transactionRepositoryMock.Verify(x => x.Add(transaction), Times.Once);
        _transactionRepositoryMock.Verify(x => x.Save(), Times.Once);

        _cacheServiceMock.Verify(x => x.Set<string>(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }
}