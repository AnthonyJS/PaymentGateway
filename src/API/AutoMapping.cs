using System;
using AutoMapper;
using PaymentGateway.API.Contracts.V1.Requests;
using PaymentGateway.API.Contracts.V1.Responses;
using PaymentGateway.Application.Commands;
using PaymentGateway.Application.Enums;
using PaymentGateway.Application.Models;

namespace PaymentGateway.API
{
  public class AutoMapping : Profile
  {
    public AutoMapping()
    {
      // From, To
      CreateMap<CreatePaymentRequest, CreatePaymentCommand>();
      CreateMap<AcquiringBankDto, CreatePaymentSuccessResponse>();
      CreateMap<CreatePaymentCommand, Payment>()
            .ForMember(dest => dest.Currency,
                   src => src.MapFrom(c => Enum.Parse(typeof(Currency), c.Currency)));
      CreateMap<Payment, PaymentByIdResponse>()
        .ForMember(dest => dest.CardNumberMasked,
                   src => src.MapFrom(c => $"____-____-____-{c.CardNumber.Substring(c.CardNumber.Length - 4)}"));
    }
  }
}
