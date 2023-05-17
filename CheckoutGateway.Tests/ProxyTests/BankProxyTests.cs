using CheckoutGateway.BusinessLogic.Proxy.Bank.Models;
using CheckoutGateway.BusinessLogic.Proxy.Bank.Service;
using Flurl.Http.Testing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System.Net;

namespace CheckoutGateway.Tests.ProxyTests;
public class BankProxyTests
{
    private readonly ILogger<BankProxy> _loggerMock;
    private readonly Mock<IOptions<BankProxyCredentials>> _bankOptionsMock;
    
    public BankProxyTests()
    {
        _loggerMock = Mock.Of<ILogger<BankProxy>>();
        _bankOptionsMock = new Mock<IOptions<BankProxyCredentials>>();
        _bankOptionsMock.SetupGet(o => o.Value).Returns(new BankProxyCredentials
        {
            Baseurl = "https://localhost:7124/api/bankpayment",
            ApiKey = "mYq3t6w9z$C&F)J@NcRfTjWnZr4u7x!A"
        });
    }

    [Fact]
    public async Task ProcessTransaction_ShouldSendRequestToBankAndReturnResponse()
    {
        // Arrange
        var reference = "123456";
        var oneTimeToken = "1111";
        var encryptedRequest = new
        {
            OneTimeToken = oneTimeToken,
            Reference = reference,
        };
        var encryptedRequestJson = JsonConvert.SerializeObject(encryptedRequest);
        var expectedResponse = new BankResponse
        {
            Message = "Success",
            Status = "00",
        };


        using (var httpTest = new HttpTest())
        {
            httpTest.RespondWithJson(expectedResponse);
            
            var bankProxy = new BankProxy(_loggerMock, _bankOptionsMock.Object);

            // Act
            var result = await bankProxy.ProcessTransaction(reference, oneTimeToken);

            // Assert
            httpTest.ShouldHaveCalled("https://localhost:7124/api/bankpayment/processtransaction")
                .WithHeader("X-Api-Key", "mYq3t6w9z$C&F)J@NcRfTjWnZr4u7x!A")
                .WithVerb(HttpMethod.Post)
                .WithRequestBody(encryptedRequestJson)
                .Times(1);

           Assert.Equal(expectedResponse, result);
        }
    }

    [Fact]
    public async Task ProcessTransaction_ValidRequest_ReturnsBankResponse()
    {
        // Arrange
        var reference = "293487";
        var oneTimeToken = "1111";
        var expectedResponse = new BankResponse
        {
            Status = "00",
            Message = "Success",
        };

        using (var httpTest = new HttpTest())
        {
            httpTest.RespondWithJson(expectedResponse);

            var bankProxy = new BankProxy(_loggerMock, _bankOptionsMock.Object);

            // Act
            var result = await bankProxy.ProcessTransaction(reference, oneTimeToken);

            // Assert
            Assert.Equal(JsonConvert.SerializeObject(expectedResponse), JsonConvert.SerializeObject(result));
        }

    }

    [Fact]
    public async Task ValidateCard_ValidRequest_ReturnsBankResponse()
    {
        // Arrange
        var cardNumber = "card-number";
        var expirationMonth = "12";
        var expirationYear = "23";
        var cvc = "123";
        var cardholderName = "Stevie Wonder";
        var amount = 100.0;

        var expectedResponse = new BankResponse
        {
            Status = "00",
            Message = "Success",
        };


        using (var httpTest = new HttpTest())
        {
            httpTest.RespondWithJson(expectedResponse);

            var bankProxy = new BankProxy(_loggerMock, _bankOptionsMock.Object);

            // Act
            var result = await bankProxy.ValidateCard(cardNumber, expirationMonth, expirationYear, cvc, cardholderName, amount);

            // Assert
            Assert.Equal(JsonConvert.SerializeObject(expectedResponse), JsonConvert.SerializeObject(result));
        }
    }
}