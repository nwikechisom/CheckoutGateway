using Azure.Core;
using CheckoutGateway.BankSimulator.Api.HelperObjects;
using CheckoutGateway.BankSimulator.Api.Models;
using CheckoutGateway.BusinessLogic.Proxy.Bank.Models;
using CheckoutGateway.BusinessLogic.Services;
using CheckoutGateway.DataLayer.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CheckoutGateway.BankSimulator.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BankPaymentController : ControllerBase
{
    private List<Card> _sessioncards = new List<Card>();

    public BankPaymentController()
    {
        //Initiliaze cards to be used
        _sessioncards = new CardHelper().testCards;
    }
    [HttpPost("verifycard")]
    [ProducesDefaultResponseType(typeof(BankResponse))]
    public IActionResult Validate([FromBody] EncryptedRequestData request)
    {
        var requestReference = Guid.NewGuid().ToString().Replace("-", "");

        var decryptedRequest = decryptRequest<VerifyCardRequest>(request);
        
        if (decryptedRequest is null)
            return Unauthorized(new BankResponse
            {
                Message = "Unauthorized request",
                Status = "99",
                Reference = requestReference
            });

        // Find the card in the list of test cards
        var card = _sessioncards.FirstOrDefault(c => c.CardNumber == decryptedRequest.CardNumber && c.ExpiryMonth == decryptedRequest.ExpirationMonth && c.ExpiryYear == decryptedRequest.ExpirationYear);

        // If the card is found, check its status
        if (card != null)
        {
            switch (card.Status)
            {
                case CardStatus.Valid:
                    if (card.Customer.Name == decryptedRequest.CardHolderName && card.Customer.AvailableBalance > decryptedRequest.Amount)
                    {
                        card.Customer.Lien = decryptedRequest.Amount;
                        return Ok(new BankResponse
                        {
                            Message = card.HolderName,
                            Status = "00",
                            Reference = requestReference
                        });
                    }
                    else
                    {
                        return BadRequest(
                            new BankResponse
                            {
                                Message = "Cardholder name does not match",
                                Status = "55",
                                Reference = requestReference
                            });
                    }
                case CardStatus.Expired:
                    return BadRequest(new BankResponse
                    {
                        Message = "Card is expired",
                        Status = "99",
                        Reference = requestReference
                    });
                case CardStatus.Frozen:
                    return BadRequest(new BankResponse
                    {
                        Message = "Card is frozen",
                        Status = "88",
                        Reference = requestReference
                    });
                case CardStatus.NotAcceptingOnlinePayments:
                    return BadRequest(new BankResponse
                    {
                        Message = "Card is not accepting online payments",
                        Status = "77",
                        Reference = requestReference
                    });
            }
        }

        // If the card is not found, return a 404 error
        return BadRequest(new BankResponse
        {
            Message = "Invalid card details",
            Status = "33",
            Reference = requestReference
        });
    }

    [HttpPost("processtransaction")]
    public IActionResult Process([FromBody] EncryptedRequestData requestData)
    {
        var decryptedRequest = decryptRequest<ProcessTransactionRequest>(requestData);
        if (decryptedRequest is null)
            return Unauthorized(new BankResponse
            {
                Message = "Unauthorized request",
                Status = "99",
                Reference = ""
            });

        switch (decryptedRequest.OneTimeToken)
        {
            case null:
                return BadRequest(new BankResponse { Message = "Provide Token", Status = "55", Reference= decryptedRequest.Reference});
            case "1111":

                return Ok(new BankResponse
                {
                    Message = "Payment Completed",
                    Status = "00",
                    Reference = decryptedRequest.Reference
                });
            default:
                return BadRequest(new BankResponse { Message = "Invalid Token", Status = "99", Reference = decryptedRequest.Reference });
        }
    }

    private T decryptRequest<T>(EncryptedRequestData requestData)
    {
        var decryptedRequest = EncryptionHelper.DecryptRequest(requestData.EncryptedData, requestData.Iv, HttpContext.Request.Headers["X-API-Key"]);
        var requestObject = JsonConvert.DeserializeObject<T>(decryptedRequest);
        return requestObject;
    }
}
