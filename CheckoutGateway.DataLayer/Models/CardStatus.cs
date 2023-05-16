namespace CheckoutGateway.DataLayer.Models;

public enum CardStatus
{
    Valid,
    Expired,
    Frozen,
    NotAcceptingOnlinePayments,
    InsufficientFunds
}
