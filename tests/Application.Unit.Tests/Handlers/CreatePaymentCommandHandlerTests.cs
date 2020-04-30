using Moq;
using Xunit;
using System;
using CSharpFunctionalExtensions;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics;
using Microsoft.Extensions.Logging;
using PaymentGateway.Application.Commands;
using PaymentGateway.Domain.AggregatesModel.PaymentAggregate;
using PaymentGateway.Domain.Enums;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Domain.Metrics;

namespace PaymentGateway.Application.Unit.Tests.Handlers
{
  [Trait("Payments", "Create")]
  public class CreatePaymentCommandHandlerTests
  {
    private readonly Mock<IAcquiringBankService> _mockAcquiringBankService;
    private readonly Mock<IPaymentHistoryRepository> _mockPaymentHistoryRepository;
    private readonly Mock<ILogger<CreatePaymentCommandHandler>> _mockLogger;
    private readonly Mock<IMetrics> _mockMetrics;
    private readonly Mock<IEventStoreClient> _mockEventStoreClient;

    public CreatePaymentCommandHandlerTests()
    {
      _mockAcquiringBankService = new Mock<IAcquiringBankService>();
      _mockPaymentHistoryRepository = new Mock<IPaymentHistoryRepository>();
      _mockLogger = new Mock<ILogger<CreatePaymentCommandHandler>>();
      _mockEventStoreClient = new Mock<IEventStoreClient>();
      _mockMetrics = new Mock<IMetrics>();
      _mockMetrics.Setup(m => m.Measure.Counter.Increment(MetricsRegistry.PaymentsCreatedCounter));
    }

    [Fact]
    public async Task ShouldSendPaymentToAcquiringBankAndSaveInDB()
    {
      var acquiringBankResult = Result.Ok(Guid.NewGuid());
      
      _mockAcquiringBankService.Setup(a => a.ProcessPayment(It.IsAny<Payment>())).ReturnsAsync(acquiringBankResult);
      _mockPaymentHistoryRepository.Setup(p => p.InsertPayment(It.IsAny<Payment>())).ReturnsAsync(Result.Ok(Guid.NewGuid()));

      var sut = new CreatePaymentCommandHandler(_mockAcquiringBankService.Object,
                      _mockPaymentHistoryRepository.Object, _mockMetrics.Object, _mockLogger.Object, _mockEventStoreClient.Object);

      var command = new CreatePaymentCommand()
      {
        Amount = 4404.44M,
        Currency = Currency.GBP.ToString(),
        CardNumber = "1234-5678-8765-4321",
        CVV = 321,
        ExpiryMonth = 10,
        ExpiryYear = 20,
        FirstName = "Jim",
        Surname = "Jimson"
      };

      var result = await sut.Handle(command, default);

      _mockAcquiringBankService.Verify(a => a.ProcessPayment(It.IsAny<Payment>()), Times.Once());
      _mockPaymentHistoryRepository.Verify(p => p.InsertPayment(It.IsAny<Payment>()), Times.Once());
      Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task ShouldSavePaymentToDatabaseEvenIfBankAcquirerDoesNotSucceed()
    {
      var acquiringBankResult = Result.Failure<Guid>("Failed to make payment on acquiring bank");

      _mockAcquiringBankService.Setup(a => a.ProcessPayment(It.IsAny<Payment>())).ReturnsAsync(acquiringBankResult);

      var sut = new CreatePaymentCommandHandler(_mockAcquiringBankService.Object,
        _mockPaymentHistoryRepository.Object, _mockMetrics.Object, _mockLogger.Object, _mockEventStoreClient.Object);

      var command = new CreatePaymentCommand()
      {
        Amount = 4404.44M,
        Currency = Currency.GBP.ToString(),
        CardNumber = "1234-5678-8765-4321",
        CVV = 321,
        ExpiryMonth = 10,
        ExpiryYear = 20,
        FirstName = "Jim",
        Surname = "Jimson"
      };

      var result = await sut.Handle(command, default);

      _mockAcquiringBankService.Verify(a => a.ProcessPayment(It.IsAny<Payment>()), Times.Once());
      _mockPaymentHistoryRepository.Verify(p => p.InsertPayment(It.IsAny<Payment>()), Times.Once());
      Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task ShouldFailIfPaymentNotSavedToDB()
    {
      var acquiringBankResult = Result.Ok<Guid>(Guid.NewGuid());

      _mockAcquiringBankService.Setup(a => a.ProcessPayment(It.IsAny<Payment>())).ReturnsAsync(acquiringBankResult);

      _mockPaymentHistoryRepository.Setup(p => p.InsertPayment(It.IsAny<Payment>()))
            .ReturnsAsync(Result.Failure<Guid>("Failed to save to DB"));

      var sut = new CreatePaymentCommandHandler(_mockAcquiringBankService.Object,
        _mockPaymentHistoryRepository.Object, _mockMetrics.Object, _mockLogger.Object, _mockEventStoreClient.Object);

      var command = new CreatePaymentCommand()
      {
        Amount = 4404.44M,
        Currency = Currency.GBP.ToString(),
        CardNumber = "1234-5678-8765-4321",
        CVV = 321,
        ExpiryMonth = 10,
        ExpiryYear = 20,
        FirstName = "Jim",
        Surname = "Jimson"
      };

      var result = await sut.Handle(command, default(CancellationToken));

      _mockAcquiringBankService.Verify(a => a.ProcessPayment(It.IsAny<Payment>()), Times.Once());
      _mockPaymentHistoryRepository.Verify(p => p.InsertPayment(It.IsAny<Payment>()), Times.Once());
      Assert.True(result.IsFailure);
    }
  }
}
