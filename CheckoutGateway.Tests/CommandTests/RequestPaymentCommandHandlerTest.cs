using AutoMapper;
using CheckoutGateway.BusinessLogic.Commands.RequestPayment;
using CheckoutGateway.BusinessLogic.Proxy.Bank.Models;
using CheckoutGateway.BusinessLogic.Proxy.Bank.Service;
using CheckoutGateway.BusinessLogic.Services.Caching;
using CheckoutGateway.DataLayer.Models;
using CheckoutGateway.DataLayer.Repositories;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;

namespace CheckoutGateway.Tests.CommandTests;

public class RequestPaymentCommandHandlerTest
{
    private readonly Mock<IGenericRepository<Transaction>> _transactionRepositoryMock;
    private readonly Mock<ILogger<RequestPaymentCommandHandler>> _loggerMock;
    private readonly Mock<IBankProxy> _bankProxyMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IValidator<RequestPaymentCommand>> _commandValidatorMock;
    private readonly RequestPaymentCommandHandler _handler;

    public RequestPaymentCommandHandlerTest()
    {
        _transactionRepositoryMock = new Mock<IGenericRepository<Transaction>>();
        _loggerMock = new Mock<ILogger<RequestPaymentCommandHandler>>();
        _bankProxyMock = new Mock<IBankProxy>();
        _cacheServiceMock = new Mock<ICacheService>();
        _mapperMock = new Mock<IMapper>();
        _commandValidatorMock = new Mock<IValidator<RequestPaymentCommand>>();

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
    public async Task Handle_ValidRequest_ReturnsValidResponse()
    {
        // Arrange
        var request = new RequestPaymentCommand
        {
            // Set the properties of the request
        };

        _commandValidatorMock.Setup(validator => validator.Validate(request))
            .Returns(new FluentValidation.Results.ValidationResult());

        _bankProxyMock.Setup(proxy => proxy.ValidateCard(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<double>()))
            .ReturnsAsync(new BankResponse { Status = "00" });

        // Mock the mapper to return a Transaction instance
        _mapperMock.Setup(mapper => mapper.Map<Transaction>(request))
            .Returns(new Transaction());

        // Mock the cache service

        // Mock the transaction repository

        // Mock the transaction repository save method

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(response);
        // Assert the properties of the response
    }
}
