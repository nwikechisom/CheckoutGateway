namespace CheckoutGateway.DataLayer.Models;

public enum TransactionStatus
{
    Success,
    Failed,
    Pending,
    Reversed,
    InvalidDetails
}
