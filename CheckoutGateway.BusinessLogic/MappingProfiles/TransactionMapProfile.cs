using AutoMapper;
using CheckoutGateway.BusinessLogic.Commands.PostPayment;
using CheckoutGateway.BusinessLogic.Commands.RequestPayment;
using CheckoutGateway.BusinessLogic.Queries.PaymentDetails;
using CheckoutGateway.DataLayer.Models;

namespace CheckoutGateway.BusinessLogic.MappingProfiles
{
    public class TransactionMapProfile : Profile
    {
        public TransactionMapProfile()
        {
            CreateMap<RequestPaymentCommand, Transaction>()
                .ForPath(dest => dest.Customer.Name, src => src.MapFrom(s => s.CardHolderName))
                .ForPath(dest => dest.Customer.Address, src => src.MapFrom(s => s.BillingAddress.Address))
                .ForPath(dest => dest.Customer.City, src => src.MapFrom(s => s.BillingAddress.City))
                .ForPath(dest => dest.Customer.PostCode, src => src.MapFrom(s => s.BillingAddress.PostCode))
                .ForPath(dest => dest.Customer.Country, src => src.MapFrom(s => s.BillingAddress.Country))
                .ForPath(dest => dest.Customer.PhoneNumber, src => src.MapFrom(s => $"{s.Phone.CountryCode}{s.Phone.Number.TrimStart(new Char[] { '0' })}"))
                .ForMember(dest => dest.Reference, src => src.MapFrom(s => s.Reference))
                .ForMember(dest => dest.Description, src => src.MapFrom(s => s.PaymentDescription))
                .ForMember(dest => dest.Amount, src => src.MapFrom(s => s.Amount))
                .ForMember(dest => dest.Currency, src => src.MapFrom(s => s.Currency))
                .ForMember(dest => dest.Merchant, src => src.MapFrom(s => s.MerchantId))
                .ForMember(dest => dest.CallBackUrl, src => src.MapFrom(s => s.Callback))
                .ReverseMap();
            CreateMap<Transaction, PostPaymentResponse>()
                .ForMember(dest => dest.Charge, src => src.MapFrom(s => s.Charge))
                .ForMember(dest => dest.Currency, src => src.MapFrom(s => s.Currency))
                .ForMember(dest => dest.Reference, src => src.MapFrom(s => s.Reference))
                .ForMember(dest => dest.Description, src => src.MapFrom(s => s.Description))
                .ForPath(dest => dest.Billing.Address, src => src.MapFrom(s => s.Customer.Address))
                .ForPath(dest => dest.Billing.Country, src => src.MapFrom(s => s.Customer.Country))
                .ForPath(dest => dest.Billing.City, src => src.MapFrom(s => s.Customer.City))
                .ForPath(dest => dest.Billing.PostCode, src => src.MapFrom(s => s.Customer.PostCode))
                .ReverseMap();
            CreateMap<Transaction, PaymentDetailQueryResponse>()
                .ForMember(dest => dest.Charge, src => src.MapFrom(s => s.Charge))
                .ForMember(dest => dest.Currency, src => src.MapFrom(s => s.Currency))
                .ForMember(dest => dest.Reference, src => src.MapFrom(s => s.Reference))
                .ForMember(dest => dest.Description, src => src.MapFrom(s => s.Description))
                .ForMember(dest => dest.Phone, src => src.MapFrom(s => s.Customer.PhoneNumber))
                .ForMember(dest => dest.Status, src => src.MapFrom(s => Enum.GetName(s.Status)))
                .ForPath(dest => dest.Billing.Address, src => src.MapFrom(s => s.Customer.Address))
                .ForPath(dest => dest.Billing.Country, src => src.MapFrom(s => s.Customer.Country))
                .ForPath(dest => dest.Billing.City, src => src.MapFrom(s => s.Customer.City))
                .ForPath(dest => dest.Billing.PostCode, src => src.MapFrom(s => s.Customer.PostCode ))
                .ReverseMap();
        }
    }
}
