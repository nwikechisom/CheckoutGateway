using CheckoutGateway.BusinessLogic.Proxy.Bank.Models;
using CheckoutGateway.BusinessLogic.Services;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace CheckoutGateway.BusinessLogic.Proxy.Bank.Service
{
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
                _logger.LogInformation("Preocess transaction request to bank with reference: {0}", reference);
                var flurlReponse = await $"{_bankOptions.Baseurl}/processtransaction"
               .WithHeader("X-Api-Key", _bankOptions.ApiKey)
               .PostJsonAsync(EncryptionHelper.EncryptRequest(new
               {
                   OneTimeToken = oneTimeToken,
                   Reference = reference,
               }, _bankOptions.ApiKey))
               .ReceiveJson<BankResponse>();
                _logger.LogInformation("ProcessTransactionBankResponse: {0}", JsonConvert.SerializeObject(flurlReponse));
                return flurlReponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("ProcessTransactionException: {0} : {1}", reference, JsonConvert.SerializeObject(ex));
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
                if (flurlReponse != null)
                {
                    return flurlReponse;
                }
                return new BankResponse
                {
                    Message = "Unable to verify card details, please try again",
                    Status = "99",
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Bank validate card exception {0}",ex);
                return new BankResponse
                {
                    Message = "Unable to verify card details, please try again",
                    Status = "99",
                };
            }
        }
    }
}
