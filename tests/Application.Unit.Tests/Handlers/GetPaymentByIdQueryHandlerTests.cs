using Moq;
using Xunit;
using System;
using CSharpFunctionalExtensions;
using System.Threading.Tasks;
using App.Metrics;
using PaymentGateway.API.Application.Queries;
using PaymentGateway.Domain.AggregatesModel.PaymentAggregate;
using PaymentGateway.Domain.Enums;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Domain.Metrics;
using PaymentGateway.Infrastructure.Persistence.PaymentHistory;

namespace PaymentGateway.Application.Unit.Tests.Handlers
{
  [Trait("Payments", "Get")]
  public class GetPaymentByIdQueryHandlerTests
  {
    private readonly Mock<IPaymentHistoryRepository> _mockPaymentHistoryRepository;
    private readonly Mock<IMetrics> _mockMetrics;

    public GetPaymentByIdQueryHandlerTests()
    {
      _mockPaymentHistoryRepository = new Mock<IPaymentHistoryRepository>();
      _mockMetrics = new Mock<IMetrics>();
      _mockMetrics.Setup(m => m.Measure.Counter.Increment(MetricsRegistry.PaymentsRetrievedCounter));
    }

    [Fact]
    public async Task ShouldFindAndReturnData()
    {
      var cardDetails = new CardDetails(
        "Jim", 
        "Jimson", 
        "1234-5678-8765-4321", 
        10, 
        20, 
        321);
      var payment = new Payment(Guid.NewGuid(), cardDetails, Currency.GBP, 123M);
      
      var paymentResult = Result.Ok(payment);
      _mockPaymentHistoryRepository.Setup(p => p.GetPaymentById(It.IsAny<Guid>())).ReturnsAsync(paymentResult);
    
      var sut = new GetPaymentByIdQueryHandler(_mockPaymentHistoryRepository.Object, _mockMetrics.Object);
    
      var query = new GetPaymentByIdQuery(Guid.NewGuid());
    
      var result = await sut.Handle(query, default);
    
      _mockPaymentHistoryRepository.Verify(p => p.GetPaymentById(It.IsAny<Guid>()), Times.Once());
      Assert.True(result.IsSuccess);
      Assert.True(result.Value.Amount == 123M);
    }
    
    [Fact]
    public async Task ShouldReturnFailureIfCannotFindPaymentById()
    {
      var paymentResult = Result.Failure<Payment>(PaymentRepositoryErrors.PaymentRetrievalFailed);
      _mockPaymentHistoryRepository.Setup(p => p.GetPaymentById(It.IsAny<Guid>())).ReturnsAsync(paymentResult);
    
      var sut = new GetPaymentByIdQueryHandler(_mockPaymentHistoryRepository.Object, _mockMetrics.Object);
    
      var query = new GetPaymentByIdQuery(Guid.NewGuid());
    
      var result = await sut.Handle(query, default);
    
      Assert.True(result.IsFailure);
    }
  }
}
