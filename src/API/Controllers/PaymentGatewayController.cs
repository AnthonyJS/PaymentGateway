using System;
using System.Threading.Tasks;
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

    public PaymentGatewayController(ILogger<PaymentGatewayController> logger, IMediator mediator)
    {
      _mediator = mediator;
      _logger = logger;
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
      // TODO: Use automapper here
      var command = new CreateCustomerOrderCommand()
      {
        Amount = request.Amount
      };

      var result = await _mediator.Send(command);

      return CreatedAtAction("CreateOrder", new { orderId = result.Id }, result);
    }
  }
}
