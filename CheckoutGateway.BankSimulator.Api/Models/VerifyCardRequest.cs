namespace CheckoutGateway.BankSimulator.Api.Models
{
    public class VerifyCardRequest
    {
        public string CardNumber { get; set; }
        public string ExpirationMonth { get; set; }
        public string ExpirationYear { get; set; }
        public string Cvv { get; set; }
        public string CardHolderName { get; set; }
        public double Amount { get; set; }
    }

    public class ProcessTransactionRequest
    {
        public string Reference { get; set;}
        public string OneTimeToken { get; set;}
    }
}
