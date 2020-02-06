using System;
using AutoMapper;
using PaymentGateway.Application.AcquiringBank.Models;
using PaymentGateway.Application.Commands;
using PaymentGateway.Application.Common.Enums;
using PaymentGateway.Application.Common.Models;
using PaymentGateway.Application.Requests;
using PaymentGateway.Application.Responses;

public class AutoMapping : Profile
{
  public AutoMapping()
  {
    // From, To
    CreateMap<CreatePaymentRequest, CreatePaymentCommand>()
      .ForMember(dest => dest.Currency,
                 src => src.MapFrom(c => Enum.Parse(typeof(Currency), c.Currency)));
    CreateMap<CreatePaymentCommand, AcquiringBankRequest>()
      .ForMember(dest => dest.Currency,
                 src => src.MapFrom(c => c.Currency.ToString()));
    CreateMap<CreatePaymentCommand, Payment>();
    CreateMap<Payment, PaymentByIdResponse>()
      .ForMember(dest => dest.CardNumber4Digits,
                 src => src.MapFrom(c => c.CardNumber.Substring(c.CardNumber.Length - 4)));
  }
}