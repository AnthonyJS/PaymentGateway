using System;
using System.Threading.Tasks;
using AutoMapper;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.API.Application.Commands;
using PaymentGateway.API.Application.Queries;
using PaymentGateway.API.Contracts.V1;
using PaymentGateway.API.Contracts.V1.Requests;
using PaymentGateway.API.Contracts.V1.Responses;
using PaymentGateway.Domain.AggregatesModel.PaymentAggregate;

namespace PaymentGateway.API.Controllers.V1
{
  [ApiController]
  public class PaymentGatewayController : ControllerBase
  {
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public PaymentGatewayController(IMediator mediator, IMapper mapper)
    {
      _mediator = mediator;
      _mapper = mapper;
    }

    /// <summary>
    /// Retrieves a Payment by Id in the query string
    /// </summary>
    /// <param name="request"></param>
    /// <response code="200">Returns the payment details</response>
    /// <response code="404">Payment could not be found</response>
    [HttpGet(ApiRoutes.Payments.Get)]
    public async Task<IActionResult> GetPayment([FromQuery]GetPaymentByIdRequest request)
    {
      var query = _mapper.Map<GetPaymentByIdQuery>(request);
      
      Result<Payment> payment = await _mediator.Send(query);

      if (!payment.IsSuccess)
        return NotFound();

      var result = _mapper.Map<GetPaymentByIdResponse>(payment.Value);

      return Ok(result);
    }

    /// <summary>
    /// Inserts a Payment using information submitted in the request body
    /// </summary>
    /// <param name="request"></param>
    /// <response code="201">Successfully inserted payment</response>
    /// <response code="400">Validation error when submitting payment</response>
    /// <response code="422">Unable to insert the payment</response>
    [HttpPost(ApiRoutes.Payments.Create)]
    public async Task<IActionResult> CreatePayment([FromBody]CreatePaymentRequest request)
    {
      var command = _mapper.Map<CreatePaymentCommand>(request);
      
      Result<Payment> payment = await _mediator.Send(command);

      if (!payment.IsSuccess)
      {
        if (payment.Error == CreatePaymentErrors.PaymentSaveFailed)
        {
          return StatusCode(StatusCodes.Status503ServiceUnavailable);
        }

        if (payment.Error == CreatePaymentErrors.AcquiringBankRefusedPayment)
        {
          return UnprocessableEntity(new {ErrorMessage = payment.Error});
        }
      }

      var result = _mapper.Map<CreatePaymentResponse>(payment.Value);

      return CreatedAtAction("CreatePayment", result, result);
    }
  }
}
