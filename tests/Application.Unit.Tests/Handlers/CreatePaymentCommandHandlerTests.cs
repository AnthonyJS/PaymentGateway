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
using PaymentGateway.Application.Commands;
using PaymentGateway.Application.Commands;
using PaymentGateway.Application.Enums;
using PaymentGateway.Application.Models;
using PaymentGateway.Application.Requests;
using PaymentGateway.Application.Responses;

namespace PaymentGateway.Application.Unit.Tests.Handlers
{
  public class AutoMapping : Profile
  {
    public AutoMapping()
    {
      // From, To
      CreateMap<CreatePaymentRequest, CreatePaymentCommand>()
        .ForMember(dest => dest.Currency,
                   src => src.MapFrom(c => Enum.Parse(typeof(Currency), c.Currency)));
      CreateMap<CreatePaymentCommand, Payment>();
      CreateMap<Payment, PaymentByIdResponse>()
        .ForMember(dest => dest.CardNumber4Digits,
                   src => src.MapFrom(c => c.CardNumber.Substring(c.CardNumber.Length - 4)));
    }
  }

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
    public async Task ShouldGetData()
    {
      var id = Guid.NewGuid();
      var acquiringBankResult = Result.Ok<Guid>(id);

      _mockAcquiringBankService.Setup(a => a.ProcessPayment(It.IsAny<Payment>())).ReturnsAsync(acquiringBankResult);

      _mockPaymentHistoryRepository.Setup(p => p.InsertPayment(It.IsAny<Payment>())).ReturnsAsync(Result.Ok<Guid>(id));

      var sut = new CreatePaymentCommandHandler(_mockAcquiringBankService.Object,
                      _mockPaymentHistoryRepository.Object, _mapper);

      var command = new CreatePaymentCommand();

      var result = await sut.Handle(command, default(CancellationToken));

      Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task ShouldReturnFailureIfBankAcquirerFails()
    {
      var id = Guid.NewGuid();
      var acquiringBankResult = Result.Failure<Guid>("Failed to make payment on acquiring bank");

      _mockAcquiringBankService.Setup(a => a.ProcessPayment(It.IsAny<Payment>())).ReturnsAsync(acquiringBankResult);

      var sut = new CreatePaymentCommandHandler(_mockAcquiringBankService.Object,
                      _mockPaymentHistoryRepository.Object, _mapper);

      var command = new CreatePaymentCommand();

      var result = await sut.Handle(command, default(CancellationToken));

      Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task ShouldReturnFailureIfPaymentNotSavedToDB()
    {
      var id = Guid.NewGuid();
      var acquiringBankResult = Result.Ok<Guid>(id);

      _mockAcquiringBankService.Setup(a => a.ProcessPayment(It.IsAny<Payment>())).ReturnsAsync(acquiringBankResult);

      _mockPaymentHistoryRepository.Setup(p => p.InsertPayment(It.IsAny<Payment>()))
            .ReturnsAsync(Result.Failure<Guid>("Failed to save to DB"));

      var sut = new CreatePaymentCommandHandler(_mockAcquiringBankService.Object,
                      _mockPaymentHistoryRepository.Object, _mapper);

      var command = new CreatePaymentCommand();

      var result = await sut.Handle(command, default(CancellationToken));

      Assert.False(result.IsSuccess);
    }
  }
}