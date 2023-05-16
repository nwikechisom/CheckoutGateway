namespace CheckoutGateway.DataLayer.Models;

public enum TransactionStatus
{
    Initiated,
    Success,
    Failed,
    Pending,
    Reversed,
    InvalidDetails
}
