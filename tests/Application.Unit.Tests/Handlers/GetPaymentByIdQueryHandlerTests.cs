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
using PaymentGateway.API;

namespace PaymentGateway.Application.Unit.Tests.Handlers
{


  public class GetPaymentByIdQueryHandlerTests
  {
    private readonly Mock<IPaymentHistoryRepository> _mockPaymentHistoryRepository;
    private readonly IMapper _mapper;

    public GetPaymentByIdQueryHandlerTests()
    {
      _mockPaymentHistoryRepository = new Mock<IPaymentHistoryRepository>();
      MapperConfiguration mapperConfig = new MapperConfiguration(cfg =>
      {
        cfg.AddProfile<AutoMapping>();
      });
      _mapper = mapperConfig.CreateMapper();
    }

    [Fact]
    public async Task ShouldFindAndReturnData()
    {
      var paymentResult = Result.Ok<Payment>(new Payment() { Amount = 123M });
      _mockPaymentHistoryRepository.Setup(p => p.GetPaymentById(It.IsAny<Guid>())).ReturnsAsync(paymentResult);

      var sut = new GetPaymentByIdQueryHandler(_mockPaymentHistoryRepository.Object, _mapper);

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

      var sut = new GetPaymentByIdQueryHandler(_mockPaymentHistoryRepository.Object, _mapper);

      var query = new GetPaymentByIdQuery(Guid.NewGuid());

      var result = await sut.Handle(query, default(CancellationToken));

      Assert.True(result.IsFailure);
    }
  }
}