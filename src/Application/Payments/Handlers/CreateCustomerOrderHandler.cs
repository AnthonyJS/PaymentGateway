using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PaymentGateway.Application.AcquiringBank.Enums;
using PaymentGateway.Application.AcquiringBank.Models;
using PaymentGateway.Application.Commands;
using PaymentGateway.Application.Common.Interfaces;
using PaymentGateway.Application.Responses;

namespace PaymentGateway.Application.Handlers
{
  public class CreateCustomerOrderHandler : IRequestHandler<CreateCustomerOrderCommand, OrderResponse>
  {
    private readonly IAcquiringBank _acquiringBank;
    public CreateCustomerOrderHandler(IAcquiringBank acquiringBank)
    {
      _acquiringBank = acquiringBank;

    }

    public async Task<OrderResponse> Handle(CreateCustomerOrderCommand request, CancellationToken cancellationToken)
    {
      var acquiringBankRequest = new AcquiringBankRequest()
      {
        Amount = request.Amount
      };

      var result = await _acquiringBank.ProcessPayment(acquiringBankRequest);

      return new OrderResponse()
      {
        Id = Guid.NewGuid(),
        HelloMessage = result.StatusCode == AcquiringBankStatusCode.Success
          ? "Succccceessss"
          : "Booo"
      };
    }
  }
}