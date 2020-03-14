using AutoMapper;
using PaymentGateway.API.Contracts.V1.Responses;
using PaymentGateway.Application.Models;

namespace PaymentGateway.API.Mapping
{
  public class DomainToResponseProfile : Profile
  {
    public DomainToResponseProfile()
    {
      CreateMap<AcquiringBankPayment, CreatePaymentSuccessResponse>();
      CreateMap<Payment, PaymentByIdResponse>()
          .ForMember(dest => dest.CardNumberMasked,
             src => src.MapFrom(c => $"____-____-____-{c.CardNumber.Substring(c.CardNumber.Length - 4)}"));
    }
  }
}
