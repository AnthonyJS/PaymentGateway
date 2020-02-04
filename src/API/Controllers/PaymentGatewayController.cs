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

    [HttpGet]
    public async Task<IActionResult> GetAllOrders()
    {
      var query = new GetAllOrdersQuery();
      var result = await _mediator.Send(query);
      return Ok(result);
    }

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
        CustomerId = request.CustomerId,
        ProductId = request.ProductId
      };

      var result = await _mediator.Send(command);

      return CreatedAtAction("CreateOrder", new { orderId = result.Id }, result);
    }
  }
}
