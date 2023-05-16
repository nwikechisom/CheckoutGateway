using CheckoutGateway.BusinessLogic.Proxy.Bank.Models;

namespace CheckoutGateway.BusinessLogic.Proxy.Bank.Service;

public interface IBankProxy
{
    Task<BankResponse> ValidateCard(string cardNumber, string expirationMonth, string expirationYear, string cvc, string cardholderName, double amount);
    Task<BankResponse> ProcessTransaction(string reference, string oneTimeToken);

}
