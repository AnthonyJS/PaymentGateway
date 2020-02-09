using System;
using CSharpFunctionalExtensions;
using MediatR;
using PaymentGateway.Application.Enums;
using PaymentGateway.Application.Models;

namespace PaymentGateway.Application.Commands
{
  public class CreatePaymentCommand : IRequest<Result<PaymentResponse>>
  {
    public string FirstName { get; set; }
    public string Surname { get; set; }
    public string CardNumber { get; set; }
    public byte ExpiryMonth { get; set; }
    public byte ExpiryYear { get; set; }
    public Currency Currency { get; set; }
    public decimal Amount { get; set; }
    public short CVV { get; set; }
  }
}