using AutoMapper;
using PaymentGateway.API.Application.Commands;
using PaymentGateway.API.Contracts.V1.Requests;

namespace PaymentGateway.API.Mapping
{
  public class RequestToCommandProfile : Profile
  {
    public RequestToCommandProfile()
    {
      CreateMap<CreatePaymentRequest, CreatePaymentCommand>();
    }
  }
}
