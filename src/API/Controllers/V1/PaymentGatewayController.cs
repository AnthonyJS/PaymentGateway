﻿using System;
using System.Threading.Tasks;
using AutoMapper;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway.API.Contracts.V1;
using PaymentGateway.API.Contracts.V1.Requests;
using PaymentGateway.API.Contracts.V1.Responses;
using PaymentGateway.Application.Commands;
using PaymentGateway.Application.Models;
using PaymentGateway.Application.Queries;

namespace PaymentGateway.API.Controllers.V1
{
  [ApiController]
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

    /// <summary>
    /// Retrieves a Payment by Id in the query string
    /// </summary>
    /// <param name="paymentId"></param>
    /// <response code="200">Returns the payment details</response>
    /// <response code="404">Payment could not be found</response>
    [HttpGet(ApiRoutes.Payments.Get)]
    public async Task<IActionResult> GetPayment(Guid paymentId)
    {
      var query = new GetPaymentByIdQuery(paymentId);
      Result<Payment> result = await _mediator.Send(query);

      return result.IsSuccess
        ? (IActionResult)Ok(_mapper.Map<PaymentByIdResponse>(result.Value))
        : NotFound();
    }

    /// <summary>
    /// Inserts a Payment using information submitted in the request body
    /// </summary>
    /// <response code="201">Successfully inserted payment</response>
    /// <response code="400">Validation error when submitting payment</response>
    /// <response code="422">Unable to insert the payment</response>
    [HttpPost(ApiRoutes.Payments.Create)]
    public async Task<IActionResult> CreatePayment([FromBody]CreatePaymentRequest request)
    {
      var command = _mapper.Map<CreatePaymentCommand>(request);

      Result<AcquiringBankPayment> result = await _mediator.Send(command);

      if (!result.IsSuccess)
        return UnprocessableEntity(new { ErrorMessage = result.Error });

      if (!result.Value.IsSuccess)
        return UnprocessableEntity(new { id = result.Value.Id, result.Value.ErrorMessage });

      var response = _mapper.Map<CreatePaymentSuccessResponse>(result.Value);

      return CreatedAtAction("CreatePayment", response, response);
    }
  }
}