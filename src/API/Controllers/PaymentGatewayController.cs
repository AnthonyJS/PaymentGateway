using System;
using System.Threading.Tasks;
using AutoMapper;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway.Application.Commands;
using PaymentGateway.Application.Models;
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
      GetPaymentByIdQuery query = new GetPaymentByIdQuery(paymentId);
      Result<PaymentByIdResponse> result = await _mediator.Send(query);

      return result.IsSuccess
        ? (IActionResult)Ok(result.Value)
        : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentRequest request)
    {
      CreatePaymentCommand command = _mapper.Map<CreatePaymentCommand>(request);

      Result<PaymentResponse> result = await _mediator.Send(command);

      if (!result.IsSuccess)
        return UnprocessableEntity(new { ErrorMessage = result.Error });

      if (!result.Value.IsSuccess)
        return UnprocessableEntity(new { id = result.Value.Id, result.Value.ErrorMessage });

      var response = new CreatePaymentCommandResponse() { Id = result.Value.Id };

      return CreatedAtAction("CreatePayment", response, response);
    }
  }
}
