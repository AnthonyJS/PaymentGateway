using AutoMapper;
using PaymentGateway.API.Contracts.V1.Responses;
using PaymentGateway.Domain.AggregatesModel.PaymentAggregate;

namespace PaymentGateway.API.Mapping
{
  public class DomainToResponseProfile : Profile
  {
    public DomainToResponseProfile()
    {
      CreateMap<Payment, CreatePaymentResponse>();
      
      CreateMap<Payment, GetPaymentByIdResponse>()
        .ForMember(dest => dest.FirstName, src => src.MapFrom(c => c.CardDetails.FirstName))
        .ForMember(dest => dest.Surname, src => src.MapFrom(c => c.CardDetails.Surname))
        .ForMember(dest => dest.CardNumberMasked,
             src => src.MapFrom(c => 
               $"____-____-____-{c.CardDetails.CardNumber.Substring(c.CardDetails.CardNumber.Length - 4)}"))
        .ForMember(dest => dest.ExpiryMonth, src => src.MapFrom(c => c.CardDetails.ExpiryMonth))
        .ForMember(dest => dest.ExpiryYear, src => src.MapFrom(c => c.CardDetails.ExpiryYear))
        .ForMember(dest => dest.CVV, src => src.MapFrom(c => c.CardDetails.CVV))
        .ForMember(dest => dest.Currency, src => src.MapFrom(c => c.Currency))
        .ForMember(dest => dest.IsSuccess, src => src.MapFrom(c => c.PaymentStatus == PaymentStatus.Success))
        .ForMember(dest => dest.ErrorMessage, src => src.MapFrom(c => c.ErrorMessage));
    }
  }
}
