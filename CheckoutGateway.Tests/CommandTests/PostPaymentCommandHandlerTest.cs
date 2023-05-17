using AutoMapper;
using CheckoutGateway.BusinessLogic.Commands.PostPayment;
using CheckoutGateway.BusinessLogic.Proxy.Bank.Models;
using CheckoutGateway.BusinessLogic.Proxy.Bank.Service;
using CheckoutGateway.BusinessLogic.Services.Caching;
using CheckoutGateway.DataLayer.Models;
using CheckoutGateway.DataLayer.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace CheckoutGateway.Tests.CommandTests;

public class PostPaymentCommandHandlerTest
{
    private Mock<IBankProxy> _bankProxyMock;
    private Mock<IGenericRepository<Transaction>> _transactionRepositoryMock;
    private Mock<ICacheService> _cacheServiceMock;
    private Mock<IMapper> _mapperMock;
    private PostPaymentCommandHandler _commandHandler;

    public PostPaymentCommandHandlerTest()
    {
        _bankProxyMock = new Mock<IBankProxy>();
        _transactionRepositoryMock = new Mock<IGenericRepository<Transaction>>();
        _cacheServiceMock = new Mock<ICacheService>();
        _mapperMock = new Mock<IMapper>();

        _commandHandler = new PostPaymentCommandHandler(
            _bankProxyMock.Object,
            _transactionRepositoryMock.Object,
            _cacheServiceMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_ValidTransaction_ReturnsPostPaymentResponse()
    {
        // Arrange
        // Create a valid transaction object
        var transaction = new Transaction
        { 
            Customer = new Customer
            {
                Email = "benmurray@muli.co"
            },
            Amount = 2000,
            Charge = 0,
            Merchant= "1345663",
            Currency ="GBP",
            CallBackUrl = "",
            Reference = "2224455",
            Description = "Handle Testing",
            Status = TransactionStatus.Success
        };

        // Create a valid command object
        var request = new PostPaymentCommand
        { 
            OneTimePassword = "1111",
            TransactionReference = "2224455",
            MerchantId = "1345663"
        }; 

        _transactionRepositoryMock.Setup(r => r.Find(It.IsAny<Expression<Func<Transaction, bool>>>(), It.IsAny<Expression<Func<Transaction, object>>>()))
            .Returns(new List<Transaction> { transaction }.AsQueryable());

        _cacheServiceMock.Setup(c => c.Get<string>(request.TransactionReference))
            .Returns("CachedBankReference");

        _bankProxyMock.Setup(b => b.ProcessTransaction("CachedBankReference", request.OneTimePassword))
            .ReturnsAsync(new BankResponse { Status="00", Message="", Reference= "CachedBankReference" });

        _transactionRepositoryMock.Setup(r => r.Update(transaction));
        _transactionRepositoryMock.Setup(r => r.Save());

        _mapperMock.Setup(m => m.Map<PostPaymentResponse>(transaction))
            .Returns(new PostPaymentResponse { Posted = true, Reference = transaction.Reference, Description = "Payment Completed" });
        // Act
        var response = await _commandHandler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.Posted); 
        Assert.Equal(request.TransactionReference, response.Reference);
        Assert.NotNull(response.Description);
    }

    [Fact]
    public async Task Handle_InvalidTransaction_ReturnsInvalidTransactionResponse()
    {
        // Arrange
        var request = new PostPaymentCommand
        {
            OneTimePassword = "1115",
            TransactionReference = "1222121",
            MerchantId = "1345663"
        }; // Create an invalid command object

        var transaction = new Transaction
        {
            Customer = new Customer
            {
                Email = "benmurray@muli.co"
            },
            Amount = 2000,
            Charge = 0,
            Merchant = "1345663",
            Currency = "GBP",
            CallBackUrl = "",
            Reference = "2224455",
            Description = "Handle Testing",
            Status = TransactionStatus.Success
        };//transaction
        _transactionRepositoryMock.Setup(r => r.Find(It.IsAny<Expression<Func<Transaction, bool>>>(), It.IsAny<Expression<Func<Transaction, object>>>()))
            .Returns(new List<Transaction> { transaction }.AsQueryable());

        // Act
        var response = await _commandHandler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(response.Posted);
        Assert.Equal(request.TransactionReference, response.Reference);
        Assert.Equal("Invalid Transaction Reference", response.Description);
    }
}
