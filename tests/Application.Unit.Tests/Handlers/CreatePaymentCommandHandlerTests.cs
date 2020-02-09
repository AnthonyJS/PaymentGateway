using PaymentGateway.Application.Queries;
using Moq;
using Xunit;
using PaymentGateway.Application.Interfaces;
using AutoMapper;
using System;
using CSharpFunctionalExtensions;
using PaymentGateway.API;
using PaymentGateway.Application.Handlers;
using System.Threading;
using System.Threading.Tasks;
using PaymentGateway.Application.Commands;
using PaymentGateway.Application.Enums;
using PaymentGateway.Application.Models;
using PaymentGateway.Application.Requests;
using PaymentGateway.Application.Responses;

namespace PaymentGateway.Application.Unit.Tests.Handlers
{
  public class CreatePaymentCommandHandlerTests
  {
    private readonly Mock<IAcquiringBankService> _mockAcquiringBankService;
    private readonly Mock<IPaymentHistoryRepository> _mockPaymentHistoryRepository;
    private readonly IMapper _mapper;

    public CreatePaymentCommandHandlerTests()
    {
      _mockAcquiringBankService = new Mock<IAcquiringBankService>();
      _mockPaymentHistoryRepository = new Mock<IPaymentHistoryRepository>();
      MapperConfiguration mapperConfig = new MapperConfiguration(cfg =>
      {
        cfg.AddProfile<AutoMapping>();
      });
      _mapper = mapperConfig.CreateMapper();
    }

    [Fact]
    public async Task ShouldSendPaymentToAcquiringBankAndSaveInDB()
    {
      var acquiringBankResult = Result.Ok<Guid>(Guid.NewGuid());

      _mockAcquiringBankService.Setup(a => a.ProcessPayment(It.IsAny<Payment>())).ReturnsAsync(acquiringBankResult);

      _mockPaymentHistoryRepository.Setup(p => p.InsertPayment(It.IsAny<Payment>())).ReturnsAsync(Result.Ok<Guid>(Guid.NewGuid()));

      var sut = new CreatePaymentCommandHandler(_mockAcquiringBankService.Object,
                      _mockPaymentHistoryRepository.Object, _mapper);

      var command = new CreatePaymentCommand();

      var result = await sut.Handle(command, default(CancellationToken));

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
                      _mockPaymentHistoryRepository.Object, _mapper);

      var command = new CreatePaymentCommand();

      var result = await sut.Handle(command, default(CancellationToken));


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
                      _mockPaymentHistoryRepository.Object, _mapper);

      var command = new CreatePaymentCommand();

      var result = await sut.Handle(command, default(CancellationToken));

      _mockAcquiringBankService.Verify(a => a.ProcessPayment(It.IsAny<Payment>()), Times.Once());
      _mockPaymentHistoryRepository.Verify(p => p.InsertPayment(It.IsAny<Payment>()), Times.Once());
      Assert.True(result.IsFailure);
    }
  }
}