using System;
using AutoMapper;
using PaymentGateway.Application.Commands;
using PaymentGateway.Application.Enums;
using PaymentGateway.Application.Models;

namespace PaymentGateway.API.Mapping
{
  public class CommandToDomainProfile : Profile
  {
    public CommandToDomainProfile()
    {
      CreateMap<CreatePaymentCommand, Payment>()
            .ForMember(dest => dest.Currency,
                   src => src.MapFrom(c => Enum.Parse(typeof(Currency), c.Currency)));
    }
  }
}
