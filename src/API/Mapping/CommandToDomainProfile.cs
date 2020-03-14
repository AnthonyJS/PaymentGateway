using AutoMapper;
using PaymentGateway.API.Contracts.V1.Requests;
using PaymentGateway.Application.Commands;

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
