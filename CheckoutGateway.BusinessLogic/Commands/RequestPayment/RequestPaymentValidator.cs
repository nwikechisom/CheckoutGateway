using FluentValidation;

namespace CheckoutGateway.BusinessLogic.Commands.RequestPayment;

public class RequestPaymentValidator : AbstractValidator<RequestPaymentCommand>
{
    public RequestPaymentValidator()
    {
        RuleFor(x => x.CardNumber).NotEmpty();
        RuleFor(x => x.CardExpiryMonth).NotEmpty().InclusiveBetween(1, 12);
        RuleFor(x => x.CardExpiryYear).NotEmpty();
        RuleFor(x => x.CardHolderName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.CardCvv).NotEmpty().Equals(3);
        RuleFor(x => x.BillingAddress.Country).NotEmpty();
        RuleFor(x => x.BillingAddress.Address).NotEmpty();
        RuleFor(x => x.Phone.CountryCode).NotEmpty();
        RuleFor(x => x.Phone.Number).NotEmpty();
        RuleFor(x => x.Callback).NotEmpty();
    }
}
