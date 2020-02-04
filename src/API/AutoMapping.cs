using AutoMapper;
using PaymentGateway.Application.AcquiringBank.Models;
using PaymentGateway.Application.Commands;
using PaymentGateway.Application.Requests;

public class AutoMapping : Profile
{
  public AutoMapping()
  {
    // From, To
    CreateMap<CreateCustomerOrderRequest, CreateCustomerOrderCommand>();
    CreateMap<CreateCustomerOrderCommand, AcquiringBankRequest>();
  }
}