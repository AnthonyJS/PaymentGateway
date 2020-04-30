using PaymentGateway.Application.Queries;
using Moq;
using Xunit;
using PaymentGateway.Application.Interfaces;
using System;
using CSharpFunctionalExtensions;
using PaymentGateway.Application.Models;
using System.Threading;
using System.Threading.Tasks;
using PaymentGateway.Domain.AggregatesModel.PaymentAggregate;

namespace PaymentGateway.Application.Unit.Tests.Handlers
{
  [Trait("Payments", "Get")]
  public class GetPaymentByIdQueryHandlerTests
  {
    private readonly Mock<IPaymentHistoryRepository> _mockPaymentHistoryRepository;

    public GetPaymentByIdQueryHandlerTests()
    {
      _mockPaymentHistoryRepository = new Mock<IPaymentHistoryRepository>();
    }

    [Fact]
    public async Task ShouldFindAndReturnData()
    {
      var paymentResult = Result.Ok<Payment>(new Payment() { Amount = 123M });
      _mockPaymentHistoryRepository.Setup(p => p.GetPaymentById(It.IsAny<Guid>())).ReturnsAsync(paymentResult);

      var sut = new GetPaymentByIdQueryHandler(_mockPaymentHistoryRepository.Object);

      var query = new GetPaymentByIdQuery(Guid.NewGuid());

      var result = await sut.Handle(query, default(CancellationToken));

      _mockPaymentHistoryRepository.Verify(p => p.GetPaymentById(It.IsAny<Guid>()), Times.Once());
      Assert.True(result.IsSuccess);
      Assert.True(result.Value.Amount == 123M);
    }


    [Fact]
    public async Task ShouldReturnFailureIfCannotFindPaymentById()
    {
      var paymentResult = Result.Failure<Payment>("Failed to get data");
      _mockPaymentHistoryRepository.Setup(p => p.GetPaymentById(It.IsAny<Guid>())).ReturnsAsync(paymentResult);

      var sut = new GetPaymentByIdQueryHandler(_mockPaymentHistoryRepository.Object);

      var query = new GetPaymentByIdQuery(Guid.NewGuid());

      var result = await sut.Handle(query, default(CancellationToken));

      Assert.True(result.IsFailure);
    }
  }
}
