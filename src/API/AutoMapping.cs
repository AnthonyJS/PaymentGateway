using System;
using AutoMapper;
using PaymentGateway.Application.Commands;
using PaymentGateway.Application.Enums;
using PaymentGateway.Application.Models;
using PaymentGateway.Application.Requests;
using PaymentGateway.Application.Responses;

namespace PaymentGateway.API
{
  public class AutoMapping : Profile
  {
    public AutoMapping()
    {
      // From, To
      CreateMap<CreatePaymentRequest, CreatePaymentCommand>()
        .ForMember(dest => dest.Currency,
                   src => src.MapFrom(c => Enum.Parse(typeof(Currency), c.Currency)));
      CreateMap<CreatePaymentCommand, Payment>();
      CreateMap<Payment, PaymentByIdResponse>()
        .ForMember(dest => dest.CardNumber4Digits,
                   src => src.MapFrom(c => c.CardNumber.Substring(c.CardNumber.Length - 4)));
    }
  }
}