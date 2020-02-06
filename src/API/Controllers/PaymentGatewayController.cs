using System;
using System.Threading.Tasks;
using AutoMapper;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway.Application.Commands;
using PaymentGateway.Application.Queries;
using PaymentGateway.Application.Requests;
using PaymentGateway.Application.Responses;

namespace PaymentGateway.API.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class PaymentGatewayController : ControllerBase
  {
    private readonly ILogger<PaymentGatewayController> _logger;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public PaymentGatewayController(ILogger<PaymentGatewayController> logger,
    IMediator mediator, IMapper mapper)
    {
      _mediator = mediator;
      _logger = logger;
      _mapper = mapper;
    }

    [HttpGet("{paymentId}")]
    public async Task<IActionResult> GetPayment(Guid paymentId)
    {
      var query = new GetPaymentByIdQuery(paymentId);
      var result = await _mediator.Send(query);

      return result != null
        ? (IActionResult)Ok(result)
        : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentRequest request)
    {
      var command = _mapper.Map<CreatePaymentCommand>(request);

      Result<Guid> result = await _mediator.Send(command);

      if (result.IsFailure)
        return UnprocessableEntity(result.Error);

      return CreatedAtAction("CreatePayment", new { id = result.Value }, result.Value);

    }
  }
}
