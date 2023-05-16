using CheckoutGateway.BusinessLogic.Proxy.Bank.Models;
using CheckoutGateway.BusinessLogic.Proxy.Bank.Service;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;


public class BankProxyTests
{
    private readonly ILogger<BankProxy> _loggerMock;
    private readonly IOptions<BankProxyCredentials> _bankOptionsMock;
    
    public BankProxyTests(IOptions<BankProxyCredentials> bankOptionsMock)
    {
        _loggerMock = Mock.Of<ILogger<BankProxy>>();
        _bankOptionsMock = bankOptionsMock;
        _bankOptionsMock = Options.Create(new BankProxyCredentials
        {
            Baseurl = "https://localhost:7124/api/bankpayment",
            ApiKey = "mYq3t6w9z$C&F)J@NcRfTjWnZr4u7x!A"
        });
    }

    [Fact]
    public async Task ProcessTransaction_ValidRequest_ReturnsBankResponse()
    {
        // Arrange
        var reference = "transaction-reference";
        var oneTimeToken = "token";
        var expectedResponse = new BankResponse
        {
            Status = "00",
            Message = "Success",
        };

        var bankProxy = new BankProxy(_loggerMock, _bankOptionsMock);

        // Act
        var result = await bankProxy.ProcessTransaction(reference, oneTimeToken);

        // Assert
        Assert.Equal(expectedResponse, result);
    }

    [Fact]
    public async Task ProcessTransaction_ExceptionThrown_ReturnsErrorResponse()
    {
        // Arrange
        var reference = "transaction-reference";
        var oneTimeToken = "token";

        var expectedResponse = new BankResponse
        {
            Status = "99",
            Message = "Unable to process transaction, please try again",
        };

        var bankProxy = new BankProxy(_loggerMock, _bankOptionsMock);

        // Act
        var result = await bankProxy.ProcessTransaction(reference, oneTimeToken);

        // Assert
        Assert.Equal(expectedResponse.Status, result.Status);
        Assert.Equal(expectedResponse.Message, result.Message);
    }

    [Fact]
    public async Task ValidateCard_ValidRequest_ReturnsBankResponse()
    {
        // Arrange
        var cardNumber = "card-number";
        var expirationMonth = "12";
        var expirationYear = "2023";
        var cvc = "123";
        var cardholderName = "John Doe";
        var amount = 100.0;

        var expectedResponse = new BankResponse
        {
            Status = "00",
            Message = "Success",
        };

        var bankProxy = new BankProxy(_loggerMock, _bankOptionsMock);

        // Act
        var result = await bankProxy.ValidateCard(cardNumber, expirationMonth, expirationYear, cvc, cardholderName, amount);

        // Assert
        Assert.Equal(expectedResponse, result);
    }

    private Mock<IHttpClientFactory> CreateHttpClientFactoryMock(BankResponse expectedResponse)
    {
        var httpClientMock = new Mock<HttpClient>();
        httpClientMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(expectedResponse)),
            });

        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        httpClientFactoryMock
            .Setup(x => x.CreateClient(It.IsAny<string>()))
            .Returns(httpClientMock.Object);

        return httpClientFactoryMock;
    }
}