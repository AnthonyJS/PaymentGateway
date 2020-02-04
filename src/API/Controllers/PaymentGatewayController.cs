using System;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway.Application.Commands;
using PaymentGateway.Application.Queries;
using PaymentGateway.Application.Requests;

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

    // TODO: Need to make sure it only returns last 4 card digits
    [HttpGet("{orderId}")]
    public async Task<IActionResult> GetOrder(Guid orderId)
    {
      var query = new GetOrderByIdQuery(orderId);
      var result = await _mediator.Send(query);

      return result != null
        ? (IActionResult)Ok(result)
        : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateCustomerOrderRequest request)
    {
      var command = _mapper.Map<CreateCustomerOrderCommand>(request);

      var result = await _mediator.Send(command);

      return CreatedAtAction("CreateOrder", new { orderId = result.Id }, result);
    }
  }
}
