using CheckoutGateway.BusinessLogic.Proxy.Bank.Models;
using CheckoutGateway.BusinessLogic.Services;
using Flurl.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CheckoutGateway.BusinessLogic.Proxy.Bank.Service;

public class BankProxy : IBankProxy
{
    private readonly ILogger<BankProxy> _logger;
    private readonly BankProxyCredentials _bankOptions;

    public BankProxy(ILogger<BankProxy> logger, IOptions<BankProxyCredentials> bankOptions)
    {
        _logger = logger;
        _bankOptions = bankOptions.Value;
    }

    public async Task<BankResponse> ProcessTransaction(string reference, string oneTimeToken)
    {
        try
        {
            _logger.LogInformation("Process transaction request to bank with reference: {requestReference}", reference);
            var flurlReponse = await $"{_bankOptions.Baseurl}/processtransaction"
           .WithHeader("X-Api-Key", _bankOptions.ApiKey)
           .PostJsonAsync(EncryptionHelper.EncryptRequest(new
           {
               OneTimeToken = oneTimeToken,
               Reference = reference,
           }, _bankOptions.ApiKey))
           .ReceiveJson<BankResponse>();
            _logger.LogInformation("ProcessTransactionBankResponse: {ProcessTransactionResponse}", JsonConvert.SerializeObject(flurlReponse));
            return flurlReponse;
        }
        catch (Exception ex)
        {
            _logger.LogError("ProcessTransactionException: {reference} : {exception}", reference, JsonConvert.SerializeObject(ex));
            return new BankResponse
            {
                Message = "Unable to process transaction, please try again",
                Status = "99",
            };
        }
    }

    public async Task<BankResponse> ValidateCard(string cardNumber, string expirationMonth, string expirationYear, string cvc, string cardholderName, double amount)
    {
        try
        {
            _logger.LogInformation("Initiating request to bank for {cardholder}", cardholderName);
            var flurlReponse = await $"{_bankOptions.Baseurl}/verifycard"
           .WithHeader("X-Api-Key", _bankOptions.ApiKey)
           .AllowAnyHttpStatus()
           .PostJsonAsync(EncryptionHelper.EncryptRequest(new
           {
               CardNumber = cardNumber,
               ExpirationMonth = expirationMonth,
               ExpirationYear = expirationYear,
               Cvv = cvc,
               CardholderName = cardholderName,
               Amount = amount
           }, _bankOptions.ApiKey))
           .ReceiveJson<BankResponse>();

            _logger.LogInformation("Initiating request to bank for {ValidateCardResponse}", flurlReponse);

            if (flurlReponse != null)
                return flurlReponse;

            return new BankResponse
            {
                Message = "Unable to verify card details, please try again",
                Status = "99",
            };
        }
        catch (Exception ex)
        {
            _logger.LogError("Bank validate card exception {ValidateCardException}", JsonConvert.SerializeObject(ex));
            return new BankResponse
            {
                Message = "Unable to verify card details, please try again",
                Status = "99",
            };
        }
    }
}
