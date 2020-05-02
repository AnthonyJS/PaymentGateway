using System;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.API.Contracts.V1.Responses;

namespace PaymentGateway.API.Contracts.V1.Requests
{
  public class GetPaymentByIdQuery : IRequest<Result<GetPaymentByIdResponse>>
  {
    [FromRoute]
    public Guid Id { get; set; }

    public GetPaymentByIdQuery() { }
    
    public GetPaymentByIdQuery(Guid id)
    {
      Id = id;
    }
  }


}


