using Xunit;
using Moq;
using PaymentGateway.Application.Commands;
using PaymentGateway.Application.Queries;
using PaymentGateway.Application.Requests;
using PaymentGateway.Application.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CSharpFunctionalExtensions;
using MediatR;
using PaymentGateway.API.Controllers;
using AutoMapper;
using System;
using System.Threading.Tasks;

namespace PaymentGateway.API.Unit.Tests
{
  public class PaymentGatewayControllerTests
  {
    private readonly Mock<ILogger<PaymentGatewayController>> _mockLogger;
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<IMapper> _mockMapper;

    [Fact]
    public async void ShouldGet()
    {
      var blah = new PaymentByIdResponse()
      {
        Id = Guid.NewGuid(),
        Amount = 54543.54M
      };

      var expectedResult = new Result.Ok(blah);

      _mockMediator.Setup(m => m.Send(It.IsAny<GetPaymentByIdQuery>)).Returns(expectedResult);

      var sut = new PaymentGatewayController(_mockLogger.Object, _mockMediator.Object, _mockMapper.Object);

      var id = Guid.NewGuid();

      var result = await sut.GetPayment(id);

      Assert.True(result == HttpStatusCode.Ok);
    }
  }
}