using AutoMapper;
using CheckoutGateway.BusinessLogic.Queries.PaymentDetails;
using CheckoutGateway.DataLayer.Models;
using CheckoutGateway.DataLayer.Repositories;
using Moq;
using System.Linq.Expressions;

namespace CheckoutGateway.Tests.CommandTests;

public class PaymentDetailQueryHandlerTests
{
    private Mock<IGenericRepository<Transaction>> _transactionRepositoryMock;
    private Mock<IMapper> _mapperMock;
    private PaymentDetailQueryHandler _queryHandler;

    
    public PaymentDetailQueryHandlerTests()
    {
        _transactionRepositoryMock = new Mock<IGenericRepository<Transaction>>();
        _mapperMock = new Mock<IMapper>();

        _queryHandler = new PaymentDetailQueryHandler(
            _transactionRepositoryMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_ValidTransactionReference_ReturnsPaymentDetailQueryResponse()
    {
        // Arrange
        var request = new PaymentDetailQuery { TransactionReference = "ABC123" };
        var transaction = new Transaction { Reference = request.TransactionReference, Description = "Unit test" };
        var expectedResponse = new PaymentDetailQueryResponse { Reference = request.TransactionReference, Description = "Unit test" };

        _transactionRepositoryMock.Setup(r => r.Find(
                It.IsAny<Expression<Func<Transaction, bool>>>(),
                It.IsAny<Expression<Func<Transaction, object>>[]>()
            ))
            .Returns(new List<Transaction> { transaction });

        _mapperMock.Setup(m => m.Map<PaymentDetailQueryResponse>(transaction))
            .Returns(expectedResponse);

        // Act
        var response = await _queryHandler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(expectedResponse.Reference, response.Reference);
        Assert.NotNull(response.Description);
        // Add assertions for other properties if needed
    }

    [Fact]
    public async Task Handle_InvalidTransactionReference_ReturnsInvalidPaymentDetailQueryResponse()
    {
        // Arrange
        var request = new PaymentDetailQuery { TransactionReference = "INVALID" };
        var expectedResponse = new PaymentDetailQueryResponse
        {
            Reference = request.TransactionReference,
            Description = "Invalid transaction Reference"
        };

        _transactionRepositoryMock.Setup(r => r.Find(
                It.IsAny<Expression<Func<Transaction, bool>>>(),
                It.IsAny<Expression<Func<Transaction, object>>[]>()
            ))
            .Returns((List<Transaction>)null);

        // Act
        var response = await _queryHandler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(expectedResponse.Reference, response.Reference);
        Assert.Equal(expectedResponse.Description, response.Description);
        // Add assertions for other properties if needed
    }
}
