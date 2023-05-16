using AutoMapper;
using CheckoutGateway.BusinessLogic.Commands.RequestPayment;
using CheckoutGateway.DataLayer.Models;

namespace CheckoutGateway.BusinessLogic.MappingProfiles
{
    public class TransactionMapProfile : Profile
    {
        public TransactionMapProfile()
        {
            CreateMap<RequestPaymentCommand, Transaction>()
                .ForMember(dest => dest.Customer.Name, src => src.MapFrom(s => s.CardHolderName))
                .ForMember(dest => dest.Customer.PhoneNumber, src => src.MapFrom(s => $"{s.Phone.CountryCode}{s.Phone.Number}"))
                .ForMember(dest => dest.Reference, src => src.MapFrom(s => s.Reference))
                .ForMember(dest => dest.Amount, src => src.MapFrom(s => s.Amount))
                .ForMember(dest => dest.Amount, src => src.MapFrom(s => s.Amount))
                .ReverseMap();
        }
    }
}
