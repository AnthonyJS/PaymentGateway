using PaymentGateway.Application.Queries;
using Moq;
using Xunit;
using PaymentGateway.Application.Interfaces;
using AutoMapper;
using System;
using CSharpFunctionalExtensions;
using PaymentGateway.Application.Models;
using PaymentGateway.Application.Handlers;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentGateway.Application.Unit.Tests.Handlers
{


  public class GetPaymentByIdQueryHandlerTests
  {
    private readonly Mock<IPaymentHistoryRepository> _mockPaymentHistoryRepository;
    private readonly Mock<IMapper> _mockMapper;

    public GetPaymentByIdQueryHandlerTests()
    {
      _mockPaymentHistoryRepository = new Mock<IPaymentHistoryRepository>();
      _mockMapper = new Mock<IMapper>();
    }

    [Fact]
    public async Task ShouldGetData()
    {
      var id = Guid.NewGuid();
      var paymentResult = Result.Ok<Payment>(new Payment());
      _mockPaymentHistoryRepository.Setup(p => p.GetPaymentById(id)).ReturnsAsync(paymentResult);

      var sut = new GetPaymentByIdQueryHandler(_mockPaymentHistoryRepository.Object, _mockMapper.Object);

      var query = new GetPaymentByIdQuery(id);

      var result = await sut.Handle(query, default(CancellationToken));

      Assert.True(result.IsSuccess);
    }


    [Fact]
    public async Task ShouldNotGetData()
    {
      var id = Guid.NewGuid();
      var paymentResult = Result.Failure<Payment>("gfgfgd");
      _mockPaymentHistoryRepository.Setup(p => p.GetPaymentById(id)).ReturnsAsync(paymentResult);

      var sut = new GetPaymentByIdQueryHandler(_mockPaymentHistoryRepository.Object, _mockMapper.Object);

      var query = new GetPaymentByIdQuery(id);

      var result = await sut.Handle(query, default(CancellationToken));

      Assert.True(result.IsFailure);
    }
  }
}